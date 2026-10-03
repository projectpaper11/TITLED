using System;
using Agents;
using Agents.Enemies;
using Agents.Player;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ReverseFlashLight", story: "[Enemy] [TargetGO] spotlight reverse", category: "Action", id: "320a0e5453a6b8c4977fd06d63de90e2")]
public partial class ReverseFlashLightAction : Action
{
    [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;
    [SerializeReference] public BlackboardVariable<GameObject> TargetGO;
    private GameObject _target;
    private IChangeLight _changer;
    
    protected override Status OnStart()
    {
        _target = TargetGO.Value;
        _changer = _target.GetComponent<IChangeLight>();
        
        Vector3 dir = (TargetGO.Value.transform.position - Enemy.Value.transform.position).normalized;
        
        _changer.ChangeSpotLight(dir);
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

