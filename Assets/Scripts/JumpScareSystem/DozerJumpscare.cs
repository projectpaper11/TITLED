using System;
using System.Threading;
using System.Threading.Tasks;
using DevLib.ServiceLocator;
using DevLib.SoundSystem.Runtime;
using DG.Tweening;
using GameSystem;
using UnityEngine;
using UnityEngine.UI;

namespace JumpScareSystem
{
    public class DozerJumpscare : AbstractJumpScare
    {
        [SerializeField] private GameObject target;
        [SerializeField] private GameObject noiseEffect;
        [SerializeField] private RectTransform vignetteEffect;
        
        [SerializeField] private GameObject jumpTarget;
        [SerializeField] private Transform monster;
        [SerializeField] private Image monsterImage;
        [SerializeField] private RectTransform vignetteEffect2;
        
        [SerializeField] private Sprite[] images;
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
                _canvas.DOFade(0f, 0.5f).OnComplete(ClearTarget);
                tween.Kill();
            });
        }

        public override void ShowJumpScare()
        {
            vignetteEffect.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.15f).SetLoops(-1, LoopType.Yoyo);
            jumpTarget.SetActive(true);
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            MonsterChangeAsync();

        }

        private void OnDestroy()
        {
            _cts?.Cancel();
        }

        private async void MonsterChangeAsync()
        {
            Tween tween = vignetteEffect2.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.15f).SetLoops(-1, LoopType.Yoyo);
            monster.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            await Task.Delay(777,_cts.Token);
            monsterImage.sprite = images[0];
            monster.localScale = new Vector3(1f, 1f, 1f);
            await Task.Delay(333,_cts.Token);
            monsterImage.sprite = images[1];
            monster.DOScale(new Vector3(20, 20, 20), 0.5f);
            await Task.Delay(400,_cts.Token);
            tween.Kill();
            noiseEffect.SetActive(true);
            ServiceLocator.Get<IAudioService>().PlaySfx(noiseSFX, 1);
            await Task.Delay(250);
            ServiceLocator.Get<ISceneService>().MoveScene(1);
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