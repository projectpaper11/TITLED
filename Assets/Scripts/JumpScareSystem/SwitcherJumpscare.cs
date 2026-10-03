using System;
using System.Threading;
using System.Threading.Tasks;
using DevLib.ServiceLocator;
using DevLib.SoundSystem.Runtime;
using DG.Tweening;
using GameSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JumpScareSystem
{
    public class SwitcherJumpscare : AbstractJumpScare
    {
        [Header("SpotEffect")]
        [SerializeField] private GameObject target;
        [SerializeField] private GameObject noiseEffect;
        [SerializeField] private RectTransform vignetteEffect;

        [Header("Jumpscare")] 
        [SerializeField] private GameObject jumpTarget;
        [SerializeField] private GameObject[] monsters;
        [SerializeField] private SoundClipSO noiseSFX;
        
        private Tween _tween;
        private CanvasGroup _canvas;
        private CancellationTokenSource _cts;
        
        
        private void Start()
        {
            ClearTarget();
            _canvas = target.GetComponent<CanvasGroup>();
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts = null;
        }

        public override void ShowSpotEffect()
        {
            SettingTarget();
            Tween tween = vignetteEffect.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.15f).SetLoops(-1, LoopType.Yoyo);
            ShowNoiseEffect();
            _tween = DOVirtual.DelayedCall(1.5f,() =>
            {
                _canvas.DOFade(0f, 0.5f).OnComplete(() =>
                {
                    ClearTarget();
                    tween.Kill();
                });
            });
        }

        public override void ShowJumpScare()
        {
            jumpTarget.SetActive(true);
            _tween?.Kill();
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            AppearMonstersAsync();
        }

        private async void AppearMonstersAsync()
        {
            for (int i = 0; i < monsters.Length; i++)
            {
                await Task.Delay(500/(i + 1),_cts.Token);
                monsters[i].SetActive(true);
            }
            Tween tween = monsters[^1].transform.DOShakeRotation(2f,Vector3.one).SetUpdate(true);
            await Task.Delay(1000,cancellationToken:_cts.Token);
            noiseEffect.SetActive(true);
            ServiceLocator.Get<IAudioService>().PlaySfx(noiseSFX, 1);
            await Task.Delay(250,cancellationToken:_cts.Token);
            tween.Kill();
            ServiceLocator.Get<ISceneService>().MoveScene(1);
        }

        public override void DeadEffect()
        {
            ShowNoiseEffect();
        }

        private void SettingTarget()
        {
            target.SetActive(true);
            _tween?.Kill();
            _canvas.alpha = 1;
        }
        
        private void ClearTarget()
        {
            _tween?.Kill();
            vignetteEffect.localScale = Vector3.one;
            target.SetActive(false);
        }

        private void ShowNoiseEffect(float interval = 0.2f)
        {
            ServiceLocator.Get<IAudioService>().PlaySfx(noiseSFX, 1);
            noiseEffect.SetActive(true);
            DOVirtual.DelayedCall(interval, () =>
            {
                ServiceLocator.Get<IAudioService>().StopSfx(1);
                noiseEffect.SetActive(false);
            });
        }
    }
}