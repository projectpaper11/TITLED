using DevLib.SoundSystem.Runtime;
using System;
using DevLib.ServiceLocator;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PlaySound", story: "Play [SoundClipSO]", category: "Action", id: "984957370fd4159ff8cad28925b46916")]
public partial class PlaySoundAction : Action
{
    [SerializeReference] public BlackboardVariable<SoundClipSO> SoundClipSO;

    protected override Status OnStart()
    {
        ServiceLocator.Get<IAudioService>().PlaySfx(SoundClipSO.Value);
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

