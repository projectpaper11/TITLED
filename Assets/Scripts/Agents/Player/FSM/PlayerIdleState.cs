using DevLib.FsmSystem.Runtime;
using UnityEngine;

namespace Agents.Player.FSM
{
    public class PlayerIdleState : AbstractPlayerState
    {
        public PlayerIdleState(GameObject owner, StateSO stateData) : base(owner, stateData)
        {
        }

        protected override bool OnUpdate()
        {
            Vector2 inputDirection = _player.PlayerInput.InputDirection;
            _player.SpotLight.SetSpotLightFacing((_player.PlayerInput.GetWorldMousePosition() - _player.transform.position).normalized);
            if (inputDirection.sqrMagnitude > MOVE_THRESHOLD)
            {
                _player.ChangeState(PlayerState.MOVE);
                return false;
            }

            return true;
        }
    }
}