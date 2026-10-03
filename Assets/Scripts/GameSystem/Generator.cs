using System;
using Agents;
using DevLib.ServiceLocator;
using EffectSystem;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GameSystem
{
    public class Generator : MonoBehaviour, ISpottable
    {
        [Header("Main")]
        [SerializeField] private float targetTime = 2;
        [Header("References")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Light2D[] lights;
        [SerializeField] private GameObject effectPrefab;
        [Header("Sprites")]
        [SerializeField] private Sprite unspottedSprite;
        [SerializeField] private Sprite spottedSprite;
        [SerializeField] private Sprite endSprite;
        [Header("LightColors")]
        [SerializeField] private Color unspottedColor = new Color32(255, 255, 255, 255);
        [SerializeField] private Color spottedColor = new Color32(255, 0, 0, 255);
        
        private float _timer;
        private bool _isActive;
        private bool _isEnable = true;
        private IGenerateEffectSystem _generateEffectSystem;

        private void Start()
        {
            _generateEffectSystem = ServiceLocator.Get<IGenerateEffectSystem>();
        }

        public void Spotted()
        {
            if (!_isEnable || _isActive) return;
            _timer = targetTime;
            _isActive = true;
            spriteRenderer.sprite = spottedSprite;
            _generateEffectSystem.StartEffect();
            foreach (Light2D light2D in lights)
            {
                light2D.color = spottedColor;
            }
        }

        public void UnSpotted()
        {
            if (!_isEnable || !_isActive) return;
            _isActive = false;
            spriteRenderer.sprite = unspottedSprite;
            _generateEffectSystem.StopEffect();
            foreach (Light2D light2D in lights)
            {
                light2D.color = unspottedColor;
            }
        }

        private void Update()
        {
            if (_isActive && _isEnable)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0)
                {
                    ServiceLocator.Get<IGenerateSystem>().Generate();
                    spriteRenderer.sprite = endSprite;
                    foreach (Light2D light2D in lights)
                    {
                        light2D.enabled = false;
                    }
                    _isEnable = false;
                    _generateEffectSystem.StopEffect();
                    GameObject effectGO = Instantiate(effectPrefab);
                    EffectPlayer effect = effectGO.GetComponent<EffectPlayer>();
                    effect.SetPositionAndPlay(transform.position);
                }
            }
        }
    }
}