using System;
using DevLib.ModuleSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Agents
{
    public abstract class AbstractAgent : ModuleOwner
    {
        public UnityEvent OnHit;
        
        public IAnimateRenderer Renderer { get; private set; }
        public ITopDownMover Mover { get; private set; }
        public bool IsDead { get; protected set; }
        
        public Vector2 FacingDirection => Renderer?.FacingDirection ?? Vector2.zero;

        protected override void InitializeModules()
        {
            base.InitializeModules();
            Renderer = GetModule<IAnimateRenderer>();
            Mover = GetModule<ITopDownMover>();
            //방어코딩
            Debug.Assert(Renderer != null, "Renderer is null." );
            Debug.Assert(Mover != null, "Mover is null." );
        }

        protected override void AfterInitializeModules()
        {
            base.AfterInitializeModules();
        }
        
        private void OnDestroy()
        {
        }

            
        public virtual void HandleDead()
        {
            IsDead = true;
        }

        // public void ApplyDamage(DamageData damageData, Vector2 hitPoint, Vector2 hitDirection, Vector2 hitNormal)
        // {
        //     ActionData.HitPoint = hitPoint;
        //     ActionData.HitNormal = hitNormal;
        //     ActionData.IsLastHitCritical = damageData.IsCritical;
        //     ActionData.LastDealer = damageData.Dealer.gameObject;
        //     ActionData.KnockbackForce = damageData.DirectedKBForce;
        //     
        //     OnHit?.Invoke();
        //     Health.TakeDamage(damageData.DamageAmount);
        // }


    }
}