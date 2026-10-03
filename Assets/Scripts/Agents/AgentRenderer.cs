using System;
using DevLib.HashDataSystem;
using DevLib.ModuleSystem;
using UnityEngine;

namespace Agents
{
    public class AgentRenderer : MonoModule, IAnimateRenderer, IAnimatorTrigger, IAfterInitModule
    {
        
        public Animator Animator { get; private set; }
        public Vector2 FacingDirection { get; private set; }

        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            Animator = GetComponent<Animator>();
        }
        
        public void AfterInit()
        {
            if (Owner.GetModule<ITopDownMover>() is { } mover)
            {
                mover.OnMovementChanged += SetMovementDirection;
            }
        }

        private void OnDestroy()
        {
            if (Owner.GetModule<ITopDownMover>() is { } mover)
            {
                mover.OnMovementChanged -= SetMovementDirection;
            }
        }

        public void SetMovementDirection(Vector2 direction)
        {
            if(Mathf.Approximately(direction.magnitude, 0))
                return;
            transform.rotation = Quaternion.Euler(0, direction.x > 0 ? 0 : -180, 0);
        }

        public void RenderClip(int clipHash)
        {
            Animator.Play(clipHash, 0 , 0);
        }

        public void RenderClipIfNotPlaying(int clipHash)
        {
            if(Animator.GetCurrentAnimatorStateInfo(0).shortNameHash != clipHash)
                RenderClip(clipHash);
        }

        #region Trigger logic

        public event Action OnAnimationEnd;
        public event Action OnDamageCast;
        public event Action OnFootstep;
        
        private void FootstepTrigger() => OnFootstep?.Invoke();
        private void AnimationEndTrigger() => OnAnimationEnd?.Invoke(); 

        private void DamageCastTrigger() => OnDamageCast?.Invoke();

        #endregion


    }
}