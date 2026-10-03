using System;
using DevLib.ModuleSystem;
using UnityEngine;

namespace Agents
{
    public class AgentMover : MonoModule, ITopDownMover
    {
        public event Action<Vector2> OnMovementChanged;
        
        [SerializeField] private float moveSpeed = 5f;

        private Vector2 _movement;
        private Rigidbody2D _rigidbody;

        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            _rigidbody = owner.GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rigidbody.linearVelocity = _movement * moveSpeed;
        }


        public void SetMovementSpeed(float speed) => moveSpeed = Mathf.Clamp(speed, 0, 15f);

        public void SetMovement(Vector2 movement)
        {
            _movement = movement.normalized;
            OnMovementChanged?.Invoke(_movement);
        }

        public void StopImmediately()
        {
            _rigidbody.linearVelocity = Vector2.zero;
            _movement = Vector2.zero;
            OnMovementChanged?.Invoke(_movement);
        }
    }
}