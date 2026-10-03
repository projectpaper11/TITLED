using Agents.Enemies;
using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveCircleMove", story: "[Enemy] move circle middle [TargetGO] for [Float] seconds", category: "Action", id: "4b040f70e84824a535b3fc5e6e652d22")]
public partial class MoveCircleMoveAction : Action
{
    [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;
    [SerializeReference] public BlackboardVariable<GameObject> TargetGO;
    [SerializeReference] public BlackboardVariable<float> Float;
    private AbstractEnemy _enemy;
    private GameObject _target;
    private int _oneMinus = 1;
    private NavModule _nav;

    private float _timer;

    protected override Status OnStart()
    {
        if (Enemy.Value == null || TargetGO.Value == null || Enemy.Value.Nav == null)
            return Status.Failure;
        _oneMinus = UnityEngine.Random.Range(0,2) == 0 ? 1 : -1;
        _enemy = Enemy.Value;
        _target = TargetGO.Value;
        _nav = _enemy.Nav;
        _timer = Float.Value;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Vector2 dir = (_enemy.transform.position - _target.transform.position).normalized;
        Vector2 clockwiseTangent = new Vector2(dir.y * _oneMinus, -dir.x * _oneMinus).normalized;
        _nav.SetDestination(Enemy.Value.transform.position + (Vector3)(clockwiseTangent));
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

