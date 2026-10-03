using System;
using DevLib.ServiceLocator;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ShowJumpscare", story: "show [Jumpscare]", category: "Action", id: "b6e8d2ba3067bf3958ef88c4fcbf780d")]
public partial class ShowJumpscareAction : Action
{
    [SerializeReference] public BlackboardVariable<string> Jumpscare;

    protected override Status OnStart()
    {
        ServiceLocator.Get<IJumpscareService>().ShowJumpscare(Jumpscare);
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

