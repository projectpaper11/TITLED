using Agents.Enemies;
using System;
using DevLib.ServiceLocator;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Enemy Stun", story: "[Enemy] has stuned. [StunFirst] [JumpscareName]", category: "Action", id: "c2ebb1558249c5fb6b7ec98f6d26259e")]
public partial class EnemyStunAction : Action
{
    [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;
    [SerializeReference] public BlackboardVariable<bool> StunFirst;
    [SerializeReference] public BlackboardVariable<string> JumpscareName;
    
    protected override Status OnStart()
    {
        if (StunFirst)
            ServiceLocator.Get<IJumpscareService>().ShowSpotEffect(JumpscareName);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }
}

