using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GameSystem
{
    public class LobbyMonsterImage : MonoBehaviour
    {
        [SerializeField] private Sprite[] monsters;
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void Update()
        {
            if (Random.Range(0, 100) == 0)
            {
                _image.sprite = monsters[Random.Range(0, monsters.Length)];
            }
        }
    }
}