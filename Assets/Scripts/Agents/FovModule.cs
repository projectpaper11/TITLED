using System;
using System.Collections.Generic;
using Agents.Enemies;
using DevLib.ModuleSystem;
using UnityEngine;

namespace Agents
{
    public class FovModule : MonoModule
    {
        private readonly struct ViewCastInfo
        {
            public readonly bool Hit;
            public readonly Vector2 Point;
            public readonly float Dist;
            public readonly float Angle;

            public ViewCastInfo(bool hit, Vector2 point, float dist, float angle)
            {
                Hit = hit;
                Point = point;
                Dist = dist;
                Angle = angle;
            }
        }

        private readonly struct EdgeInfo
        {
            public readonly Vector2 PointA, PointB;
            public readonly bool HasPointA, HasPointB;

            public EdgeInfo(Vector2 pointA, bool hasA, Vector2 pointB, bool hasB)
            {
                PointA = pointA;
                PointB = pointB;
                HasPointA = hasA;
                HasPointB = hasB;
            }
        }
        [Header("시야 범위")]
        [SerializeField] private float viewRadius = 6f;
        [SerializeField] private float nearRadius = 1.8f;
        [Range(0f, 360f), SerializeField] private float viewAngle = 140f;

        [Header("시야 검출 값")] 
        [SerializeField] private LayerMask whatIsObstacle;
        [SerializeField] private LayerMask whatIsEnemy;
        [SerializeField] private float meshResolution = 1f; //1도당 발사될 레이의 갯수

        [Tooltip("경계를 2진 분할로 다듬는 횟수")] 
        [SerializeField] private int edgeResolveIteration = 4;

        [Tooltip("경계로 판단할 2개의 Raycast간의 임계치")] 
        [SerializeField] private float edgeDistanceThreshold = 0.05f;

        private AbstractAgent _agent;
        
        private readonly List<Vector3> _viewPoints = new List<Vector3>();
        public IReadOnlyList<Vector3> ViewPoints => _viewPoints; //렌더러가 읽을 값, 렌더러는 값을 변화 시키면 안된다.
        
        public Vector3 Origin => transform.position;
        
        public float ViewRadius => viewRadius;
        public float NearRadius => nearRadius;
        public float ViewAngle => viewAngle;
        public Vector2 Facing => GetFacingDirection();
        
        //이번 스캔에서 보였는가를 담는다. 스캔이 끝나면 false로 남은 애들이 시야에서 사라진 애들이다.
        private readonly Dictionary<ISpottable,bool> _spottables = new Dictionary<ISpottable, bool>();
        private readonly List<ISpottable> _spottableBuffer = new List<ISpottable>(); //순회 도중 수정하려고 쓰는 버퍼

        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            _agent = owner as AbstractAgent;
        }

        private void Update()
        {
            Scan(); //나중에 변경
        }

        private void Scan()
        {
            BeginSpotFrame();
            CalculateViewPoints();
            EndSpotFrame();
        }

        private void BeginSpotFrame()
        {
            if (_spottables.Count == 0) return;

            _spottableBuffer.Clear();
            _spottableBuffer.AddRange(_spottables.Keys);
            foreach (ISpottable spottable in _spottableBuffer)
                _spottables[spottable] = false;
        }

        private void EndSpotFrame()
        {
            if (_spottables.Count == 0) return;

            _spottableBuffer.Clear();
            foreach (KeyValuePair<ISpottable, bool> pair in _spottables)
            {
                if (pair.Value == false)
                    _spottableBuffer.Add(pair.Key);
            }

            foreach (ISpottable spottable in _spottableBuffer)
            {
                _spottables.Remove(spottable);

                if (spottable is UnityEngine.Object obj && obj == null) continue;

                spottable.UnSpotted();
            }
        }

        private void CalculateViewPoints()
        {
            float centerAngle = AngleFromDir(GetFacingDirection());
            int rayCount = Mathf.Max(1, Mathf.RoundToInt(viewAngle * meshResolution));
            float angleStep = viewAngle / rayCount;
            float startAngle = centerAngle - viewAngle * 0.5f;
            
            _viewPoints.Clear();

            ViewCastInfo prevCast = default;
            
            for (int i = 0; i <= rayCount; i++)
            {
                float angle = startAngle + angleStep * i;
                ViewCastInfo castInfo = ViewCast(angle);

                if (i > 0) //이전에 쏜 레이가 있다 라는 얘기
                {
                    bool thresholdExceeded = Mathf.Abs(prevCast.Dist - castInfo.Dist) > edgeDistanceThreshold;

                    if (prevCast.Hit != castInfo.Hit || (prevCast.Hit && castInfo.Hit && thresholdExceeded))
                    {
                        EdgeInfo edge = FindEdge(prevCast, castInfo);
                        if (edge.HasPointA) _viewPoints.Add(edge.PointA);
                        if (edge.HasPointB) _viewPoints.Add(edge.PointB);
                    }
                }
                _viewPoints.Add(castInfo.Point);
                prevCast = castInfo;
            }
        }

        private EdgeInfo FindEdge(ViewCastInfo minCast, ViewCastInfo maxCast)
        {
            float minAngle = minCast.Angle;
            float maxAngle = maxCast.Angle;
            Vector2 minPoint = minCast.Point;
            Vector2 maxPoint = maxCast.Point;
            bool hasMin = false, hasMax = false;

            for (int i = 0; i < edgeResolveIteration; i++)
            {
                float angle = (minAngle + maxAngle) * 0.5f;
                ViewCastInfo cast = ViewCast(angle);
                
                bool thresholdExceeded = Mathf.Abs(minCast.Dist - cast.Dist) > edgeDistanceThreshold;

                if (cast.Hit == minCast.Hit && !thresholdExceeded)
                {
                    minAngle = angle;
                    minPoint = cast.Point;
                    hasMin = true;
                }
                else
                {
                    maxAngle = angle;
                    maxPoint = cast.Point;
                    hasMax = true;
                }
            }

            return new EdgeInfo(minPoint, hasMin, maxPoint, hasMax);
        }

        private ViewCastInfo ViewCast(float globalAngleDeg)
        {
            Vector2 dir = DirFromAngle(globalAngleDeg);

            RaycastHit2D hit = Physics2D.Raycast(Origin, dir, viewRadius, whatIsObstacle);
            RaycastHit2D hitEnemy = Physics2D.Raycast(Origin, dir, viewRadius, whatIsEnemy);

            if (hitEnemy.collider != null && hitEnemy.collider.TryGetComponent<ISpottable>(out ISpottable spottable))
            {
                if (_spottables.ContainsKey(spottable) == false)
                    spottable.Spotted();

                _spottables[spottable] = true;
            }
            
            if (hit.collider != null)
            {
                return new ViewCastInfo(true, hit.point, hit.distance, globalAngleDeg);
            }

            return new ViewCastInfo(false, (Vector2) Origin + dir * viewRadius, viewRadius, globalAngleDeg);
        }


        #region Helper method
        
        private Vector2 GetFacingDirection()
        {
            Vector2 facing = transform.up;
            return facing;
        }

        private static float AngleFromDir(Vector2 dir)
        {
            return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }

        private static Vector2 DirFromAngle(float angleDeg)
        {
            float rad = angleDeg * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        }

        #endregion
        

        #region Debug region
        
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, viewRadius);
            
            Vector3 origin = transform.position;
            Vector2 facing = Application.isPlaying ? GetFacingDirection() : transform.up;
            
            float center = AngleFromDir(facing); //중심 각
            Vector3 left = DirFromAngle(center - viewAngle * 0.5f);
            Vector3 right = DirFromAngle(center + viewAngle * 0.5f);

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(origin,origin + left * viewRadius);
            Gizmos.DrawLine(origin,origin + right * viewRadius);

            if (Application.isPlaying)
            {
                Gizmos.color = Color.green;

                foreach (Vector3 point in ViewPoints)
                {
                    Gizmos.DrawLine(Origin, point);
                }
            }
        }
        #endif
        #endregion
    }
}