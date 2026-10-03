using System;
using System.Collections;
using UnityEngine;

namespace EffectSystem
{
    public class EffectPlayer : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        private float _duration;
        private WaitForSeconds _wait;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _duration = _particleSystem.main.duration;
            _wait = new WaitForSeconds(_duration);
        }

        public void SetPositionAndPlay(Vector3 position)
        {
            transform.position = position;
            _particleSystem.Play();
            StartCoroutine(ParticleRemoveRoutine());
        }

        private IEnumerator ParticleRemoveRoutine()
        {
            yield return _wait;
            _particleSystem.Stop();
            Destroy(gameObject);
        }
    }
}