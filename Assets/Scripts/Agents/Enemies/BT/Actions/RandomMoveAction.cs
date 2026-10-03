using Agents.Enemies;
using System;
using DevLib.ServiceLocator;
using DevLib.TileAstar;
using GameSystem.GameServices;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Random move", story: "[Enemy] random move to [TargetGO] [PathBakeData]", category: "Action", id: "e8f5346558ccfd28db3452592296a4f0")]
public partial class RandomMoveAction : Action
{
    [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;
    [SerializeReference] public BlackboardVariable<GameObject> TargetGO;
    [SerializeReference] public BlackboardVariable<PathBakeDataSO> PathBakeData;
    private GameObject _target;
    private AbstractEnemy _enemy;
    private IMapService _mapService;

    protected override Status OnStart()
    {
        if (TargetGO.Value == null)
            return Status.Failure;
        _target = TargetGO.Value;
        _enemy = Enemy.Value;
        _mapService = ServiceLocator.Get<IMapService>();
        float distance = Vector3.Distance(Enemy.Value.transform.position, TargetGO.Value.transform.position);
        Vector3 safePosition = new Vector3();
        NodeData node;
        do
        {
            float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector2 randomDir = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
            safePosition = _target.transform.position + (Vector3)(randomDir.normalized * distance);
        } while (!PathBakeData.Value.GetNodeIfExist(_mapService.GetWorldToCell(safePosition),out node));
        _enemy.transform.position = node.worldPosition;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

