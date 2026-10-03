using DevLib.FsmSystem.Runtime;
using DevLib.ServiceLocator;
using GameSystem.GameServices;
using UnityEngine;

namespace Agents.Player.FSM
{
    public class PlayerMoveState : AbstractPlayerState
    {
        private IAnimatorTrigger _trigger;
        public PlayerMoveState(GameObject owner, StateSO stateData) : base(owner, stateData)
        {
            _trigger = _player.GetModule<IAnimatorTrigger>();
        }

        override public void Enter()
        {
            base.Enter();
            if (_trigger != null)
                _trigger.OnFootstep += HandleFootstepEvent;
        }

        protected override bool OnUpdate()
        {
            Vector2 inputDirection = _player.PlayerInput.InputDirection;
            if (inputDirection.sqrMagnitude < MOVE_THRESHOLD)
            {
                _player.ChangeState(PlayerState.IDLE);
                return false;
            }
            
            _player.Mover.SetMovement(inputDirection);
            _player.Renderer.SetMovementDirection(inputDirection);
            _player.SpotLight.SetSpotLightFacing((_player.PlayerInput.GetWorldMousePosition() - _player.transform.position).normalized);
            
            return true;
        }

        public override void Exit()
        {
            if (_trigger != null)
                _trigger.OnFootstep -= HandleFootstepEvent;
            _player.Mover.StopImmediately();
            base.Exit();
        }

        private void HandleFootstepEvent()
        {
            ServiceLocator.Get<IMapService>()?.EnterSoundTile(_player.transform.position);
        }
    }
}