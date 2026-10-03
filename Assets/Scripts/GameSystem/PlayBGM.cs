using System;
using DevLib.ServiceLocator;
using DevLib.SoundSystem.Runtime;
using UnityEngine;

namespace GameSystem
{
    public class PlayBGM : MonoBehaviour
    {
        [SerializeField] private SoundClipSO bgmClip;

        private void Start()
        {
            ServiceLocator.Get<IAudioService>().PlayBgm(bgmClip);
        }
    }
}