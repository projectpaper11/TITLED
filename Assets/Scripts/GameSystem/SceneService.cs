using System;
using System.Threading;
using System.Threading.Tasks;
using DevLib.ServiceLocator;
using DevLib.SoundSystem.Runtime;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameSystem
{
    public class SceneService : MonoBehaviour, ISceneService
    {
        [SerializeField] private GameObject NoiseEffect;
        [SerializeField] private SoundClipSO noiseSFX;
        
        private CancellationTokenSource _cts;
        private void Awake()
        {
            ServiceLocator.Register<ISceneService>(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.UnRegister<ISceneService>(this);
            _cts?.Cancel();
        }

        public void MoveSceneWithNoise(int id)
        {
            Time.timeScale = 1;
            _cts = new CancellationTokenSource();
            NoiseEffectAsync(id);
        }

        private async void NoiseEffectAsync(int id)
        {
            NoiseEffect.SetActive(true);
            ServiceLocator.Get<IAudioService>().PlaySfx(noiseSFX, 1);
            await Task.Delay(500,cancellationToken:_cts.Token);
            SceneManager.LoadScene(id);
        }

        public void MoveScene(int id)
        {
            SceneManager.LoadScene(id);
            DOTween.KillAll();
        }
    }
}