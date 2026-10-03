using System;
using Agents;
using DevLib.ServiceLocator;
using EffectSystem;
using UnityEngine;

namespace GameSystem
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private GameObject doorVisual;
        [SerializeField] private Collider2D blockColl;
        [SerializeField] private GameObject sparklePrefab;
        private Collider2D _coll;

        private void Awake()
        {
            _coll = GetComponent<Collider2D>();
        }

        public void Close()
        {
            blockColl.isTrigger = false;
            doorVisual.SetActive(true);
            GameObject sparkleGO = Instantiate(sparklePrefab, transform.position, transform.rotation);
            EffectPlayer effect = sparkleGO.GetComponent<EffectPlayer>();
            effect.SetPositionAndPlay(transform.position);
        }

        public void Open()
        {
            blockColl.isTrigger = true;
            doorVisual.SetActive(false);
            
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _coll.enabled = false;
                ServiceLocator.Get<IOpenDoorSystem>().DoorNext(this);
            }
        }
    }
}