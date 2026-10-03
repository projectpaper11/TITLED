using System;
using System.Threading;
using System.Threading.Tasks;
using DevLib.ServiceLocator;
using DevLib.SoundSystem.Runtime;
using DG.Tweening;
using GameSystem;
using UnityEngine;

namespace JumpScareSystem
{
    public class NoiseJumpscare : AbstractJumpScare
    {
        [SerializeField] private GameObject target;
        [SerializeField] private GameObject noiseEffect;
        [SerializeField] private RectTransform vignetteEffect;

        [SerializeField] private GameObject jumpTarget;
        [SerializeField] private GameObject monster;
        [SerializeField] private GameObject shadow;
        [SerializeField] private SoundClipSO noiseSFX;
        
        
        private Tween _tween;
        private CanvasGroup _canvas;
        private CancellationTokenSource _cts;
        
        private void Start()
        {
            ClearTarget();
            _canvas = target.GetComponent<CanvasGroup>();
        }

        public override void ShowSpotEffect()
        {
            SettingTarget();
            
            Tween tween = vignetteEffect.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.15f).SetLoops(-1, LoopType.Yoyo);
            ShowNoiseEffect();
            _tween = DOVirtual.DelayedCall(1.5f,() =>
            {
                tween.Kill();
                _canvas.DOFade(0f, 0.5f).OnComplete(ClearTarget);
            });
        }

        public override void ShowJumpScare()
        {
             _tween?.Kill();
             jumpTarget.SetActive(true);
            _cts?.Cancel();
             _cts = new CancellationTokenSource();
             MonsterShakeAsync();
        }

        private async void MonsterShakeAsync()
        {
            monster.transform.DOShakePosition(3f,50f,50,fadeOut:false);
            shadow.transform.DOShakePosition(3f,100f,50,fadeOut:false);
            await Task.Delay(1500,_cts.Token);
            jumpTarget.transform.rotation = Quaternion.Euler(0, 0, 180);
            await Task.Delay(1500,_cts.Token);
            noiseEffect.SetActive(true);
            ServiceLocator.Get<IAudioService>().PlaySfx(noiseSFX, 1);
            await Task.Delay(250);
            ServiceLocator.Get<ISceneService>().MoveScene(1);
        }
        
        private void OnDestroy()
        {
            _cts?.Cancel();
        }

        public override void DeadEffect()
        {
            ShowNoiseEffect();
        }

        private void SettingTarget()
        {
            target.SetActive(true);
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