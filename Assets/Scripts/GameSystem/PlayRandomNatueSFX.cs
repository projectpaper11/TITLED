using System;
using DevLib.ServiceLocator;
using DevLib.SoundSystem.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameSystem
{
    public class PlayRandomNatueSFX : MonoBehaviour
    {
        [SerializeField] private SoundClipSO[] sfxClip;

        private float _timer = 10f;
        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                _timer = 1f;
                if (Random.Range(0f, 40f) <= 1)
                {
                    ServiceLocator.Get<IAudioService>().PlaySfx(sfxClip[Random.Range(0, sfxClip.Length)]);
                }
            }
        }
    }
}