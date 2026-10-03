using System;
using DevLib.ServiceLocator;
using UnityEngine;

namespace EffectSystem
{
    public class NoiseEffectSystem : MonoBehaviour, INoiseEffectSystem
    {
        private void Awake()
        {
            ServiceLocator.Register<INoiseEffectSystem>(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.UnRegister<INoiseEffectSystem>(this);
        }
        
    }
}