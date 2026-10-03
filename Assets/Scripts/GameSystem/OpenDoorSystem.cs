using System;
using DevLib.ServiceLocator;
using UnityEngine;

namespace GameSystem
{
    public class OpenDoorSystem : MonoBehaviour, IOpenDoorSystem
    {
        [SerializeField] private int needDoorCount;
        [SerializeField] private GameObject[] doorVisuals;
        [SerializeField] private Collider2D[] doorColliders;
        private int _currentDoorCount;

        private bool _isEnableDoors;
        private void Awake()
        {
            ServiceLocator.Register<IOpenDoorSystem>(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.UnRegister<IOpenDoorSystem>(this);
        }

        public void EnableDoors()
        {
            _isEnableDoors = true;
            foreach (GameObject visual in doorVisuals)
            {
                visual.SetActive(false);
            }

            foreach (Collider2D coll in doorColliders)
            {
                coll.enabled = true;
            }
        }

        public void DoorNext(Door currentDoor)
        {
            if (!_isEnableDoors) return;
            _currentDoorCount++;
            if (_currentDoorCount < needDoorCount)
            {
                currentDoor.Close();
            }
            else
            {
                currentDoor.Open();
            }
        }
    }
}