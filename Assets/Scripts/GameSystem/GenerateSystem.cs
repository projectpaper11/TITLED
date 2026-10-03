using System;
using System.Threading;
using System.Threading.Tasks;
using DevLib.ServiceLocator;
using DevLib.SoundSystem.Runtime;
using DG.Tweening;
using Febucci.UI;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem
{
    public class GenerateSystem : MonoBehaviour,IGenerateSystem, IGenerateEffectSystem
    {
        [SerializeField] private int needGenerate;
        [SerializeField] private Image vignetteImage;
        [SerializeField] private Transform vignetteTransform;
        [SerializeField] private TypewriterByCharacter writer;
        [SerializeField] private SoundClipSO clip;

        private int _currentGenerate;
        private Tween _tween;
        private Tween _tween2;
        private CancellationTokenSource _cts;

        private void Awake()
        {
            ServiceLocator.Register<IGenerateSystem>(this);
            ServiceLocator.Register<IGenerateEffectSystem>(this);
        }

        private void Start()
        {
            Color pickColor = vignetteImage.color;
            pickColor.a = 0;
            vignetteImage.color = pickColor;
        }

        private void OnDestroy()
        {
            ServiceLocator.UnRegister<IGenerateSystem>(this);
            ServiceLocator.UnRegister<IGenerateEffectSystem>(this);
            _cts?.Cancel();
        }

        public void Generate()
        {
            _currentGenerate++;
            ServiceLocator.Get<IAudioService>().PlaySfx(clip);
            if (_currentGenerate >= needGenerate)
            {
                ServiceLocator.Get<IOpenDoorSystem>().EnableDoors();
                writer.ShowText($"문이 열렸습니다...");
                GuideTextAsync();
            }

            else
            {
                writer.ShowText($"남은 제어기 : {_currentGenerate}/{needGenerate}");
                GuideTextAsync();
            }
        }

        private async void GuideTextAsync()
        {
            _cts = new CancellationTokenSource();
            await Task.Delay(2000,_cts.Token);
            writer.ShowText("");
        }

        public void StartEffect()
        {
            vignetteTransform.gameObject.SetActive(true);
            _tween2.Kill();
            vignetteTransform.localScale = Vector3.one;
            _tween2 = vignetteImage.DOFade(1, 1f);
            _tween = vignetteTransform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.15f).SetLoops(-1, LoopType.Yoyo);

        }

        public void StopEffect()
        {
            _tween.Kill();
            _tween2.Kill();
            _tween2 = vignetteImage.DOFade(0, 1f).OnComplete(() =>
            {
                vignetteTransform.gameObject.SetActive(false);
                vignetteTransform.localScale = Vector3.one;
            });
        }
    }
}