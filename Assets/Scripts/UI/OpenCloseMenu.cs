using System;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using Febucci.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public class OpenCloseMenu : MonoBehaviour
    {
        [SerializeField] private RectTransform handUI;
        [SerializeField] private Volume targetVolume;
        [SerializeField] private Image lightImage;
        [SerializeField] private TypewriterByCharacter writer;
        [SerializeField] private string[] ments;
        private Vector3 newPos;
        private Vector3 originPos;
        private Sequence _tween;
        private bool _isEnable;
        private ColorAdjustments _colorAdjustments;
        private CancellationTokenSource _cts;

        private void Start()
        {
            originPos = handUI.position;
            newPos = handUI.position + new Vector3(0, handUI.sizeDelta.y + 300, 0);
            handUI.position = newPos;
            targetVolume.profile.TryGet<ColorAdjustments>(out _colorAdjustments);
            lightImage.enabled = false;
            writer.onTextShowed.AddListener(HandleTextShowed);
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
        }

        private void OpenUI()
        {
            _cts = new CancellationTokenSource();
            HandleDisappearText();
            writer.onTextDisappeared.AddListener(HandleDisappearText);
            _tween?.Kill();
            _tween = DOTween.Sequence();
            Time.timeScale = 0;
            DOTween.To(() => _colorAdjustments.postExposure.value, x => _colorAdjustments.postExposure.value = x, -5, 0.5f).SetUpdate(true);
            _tween.Append(handUI.DOMove(originPos, 0.5f).SetEase(Ease.OutBack)).SetUpdate(true);
            _tween.AppendInterval(0.25f).SetUpdate(true);
            _tween.AppendCallback(() =>
            {
                lightImage.enabled = true;
            }).SetUpdate(true);
            _tween.Play();
        }

        private void HandleDisappearText()
        {
            DisappearTextAsync();
        }

        private async void DisappearTextAsync()
        {
            await Task.Delay(1000,_cts.Token);
            writer.ShowText(ments[Random.Range(0, ments.Length)]);
        }

        private void CloseUI()
        {
            _tween?.Kill();
            _tween = DOTween.Sequence();
            _cts?.Cancel();
            Time.timeScale = 1;
            DOTween.To(() => _colorAdjustments.postExposure.value, x => _colorAdjustments.postExposure.value = x, 0, 0.5f).SetUpdate(true);
            _tween.AppendCallback(() =>
            {
                lightImage.enabled = false;
                writer.SkipTypewriter();
                writer.onTextDisappeared.RemoveListener(HandleDisappearText);
                writer.StartDisappearingText();
            }).SetUpdate(true);
            _tween.Append(handUI.DOMove(newPos, 0.5f).SetEase(Ease.InBack)).SetUpdate(true);
            _tween.Play();
        }

        private void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                if (_isEnable)
                {
                    CloseUI();
                    _isEnable = false;
                }
                else
                {
                    OpenUI();
                    _isEnable = true;
                }
            }
        }

        private void HandleTextShowed()
        {
            writer.StartDisappearingText();
        }
    }
}