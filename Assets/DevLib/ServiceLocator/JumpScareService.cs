using System;
using System.Collections.Generic;
using JumpScareSystem;
using UnityEngine;

namespace DevLib.ServiceLocator
{
    public class JumpScareService : MonoBehaviour, IJumpscareService
    {
        [SerializeField] private List<AbstractJumpScare> jumpScareList;
        private Dictionary<string, AbstractJumpScare> _jumpScareDict;
        private void Awake()
        {
            ServiceLocator.Register<IJumpscareService>(this);
            _jumpScareDict = new Dictionary<string, AbstractJumpScare>();
            foreach (AbstractJumpScare jumpScare in jumpScareList)
            {
                _jumpScareDict.Add(jumpScare.GetType().Name, jumpScare);
            }
        }

        private void OnDestroy()
        {
            ServiceLocator.UnRegister<IJumpscareService>(this);
        }

        public void ShowJumpscare(string targetJumpName) => _jumpScareDict[targetJumpName].ShowJumpScare();
        public void ShowSpotEffect(string targetJumpName) => _jumpScareDict[targetJumpName].ShowSpotEffect();
        public void DeadEffect(string targetJumpName) => _jumpScareDict[targetJumpName].DeadEffect();
    }
}