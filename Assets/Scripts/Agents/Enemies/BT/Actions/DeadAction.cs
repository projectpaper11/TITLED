using Agents.Enemies;
using System;
using DevLib.ServiceLocator;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "DeadAction", story: "[Enemy] has dead. [JumpscareName]", category: "Action", id: "1ab20d5350b62f2d7ba17180599d7a7c")]
public partial class DeadAction : Action
{
    [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;
    [SerializeReference] public BlackboardVariable<string> JumpscareName;
    protected override Status OnStart()
    {
        Enemy.Value.HandleDead();
            ServiceLocator.Get<IJumpscareService>().DeadEffect(JumpscareName);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Running;
    }
}

