using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Agents
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(FovModule))]
    public class FovRenderer : MonoBehaviour
    {
        [SerializeField] private Light2D sightLight;
        [SerializeField] private Light2D nearSightLight;
        private const int NearDiscSegments = 32;
        private FovModule _fov;
        private Mesh _mesh;
        
        
        //배열로 하면 매번 그릴때마다 배열할당이 일어남.
        //리스트는 Clear를 한다고 해도 재할당이 되는게 아님 Count와 Capacity
        //
        private readonly List<Vector3> _vertices = new List<Vector3>();
        private readonly List<int> _triangles = new List<int>();
        
        

        private void Awake()
        {
            _fov = GetComponent<FovModule>();
            _mesh = new Mesh { name = "Fov Mesh" };
            GetComponent<MeshFilter>().mesh = _mesh;
        }

        //Update에서 FOV의 ViewPoint들이 생성될꺼니까 그리는건 Late에서 작업함.
        private void LateUpdate()
        {
            if (_fov == null) return;
            DrawFieldOfView(_fov.ViewPoints);
            RotateSightLight();
        }

        private void RotateSightLight()
        {
            if (sightLight == null) return; //기본적으로 spot라잍는 상단 방향으로 향한다.
            float z = Vector2.SignedAngle(Vector2.up, _fov.Facing); //up을 Facing을 바라보게 하는 회전 각
            sightLight.transform.rotation = Quaternion.Euler(0, 0, z);
        }

        private void DrawFieldOfView(IReadOnlyList<Vector3> points)
        {
            int count = points.Count;
            if (count < 2)
            {
                _mesh.Clear();
            }
            
            _vertices.Clear();
            _triangles.Clear();
            
            //월드 좌표를 이 트랜스폼의 좌표로 변환해야한다(Raycast좌표는 월드 좌표인데, vertex좌표는 로컬 좌표임)
            _vertices.Add(transform.InverseTransformPoint(_fov.Origin));
            for (int i = 0; i < count; i++)
                _vertices.Add(transform.InverseTransformPoint(points[i]));

            for (int i = 0; i < count - 1; i++)
            {
                _triangles.Add(0);
                _triangles.Add(i + 1);
                _triangles.Add(i + 2);
            }

            AppendNearDiscSight();
            
            _mesh.Clear();
            _mesh.SetVertices(_vertices);
            _mesh.SetTriangles(_triangles, 0);
            // _mesh.RecalculateNormals(); 이걸 직접 그려서 라이팅을 처리하지 않고, 이건 마스킹용으로만 쓸거라서
        }

        private void AppendNearDiscSight()
        {
            float nearR = _fov.NearRadius;
            if (nearR <= 0f) return;
            
            int rim = _vertices.Count;

            for (int i = 0; i <= NearDiscSegments; i++)
            {
                float angle = i / (float)NearDiscSegments * (Mathf.PI * 2f);
                Vector3 world = _fov.Origin + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle) * nearR, 0);
                
                _vertices.Add(transform.InverseTransformPoint(world));
            }
            
            for (int i = 0; i < NearDiscSegments; i++)
            {
                _triangles.Add(0);
                _triangles.Add(i+ rim);
                _triangles.Add(i + 1 + rim);
            }
        }
    }
}