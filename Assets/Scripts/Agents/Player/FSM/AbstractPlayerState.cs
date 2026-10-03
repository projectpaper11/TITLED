using DevLib.FsmSystem.Runtime;
using UnityEngine;

namespace Agents.Player.FSM
{
    public abstract class AbstractPlayerState : AbstractState
    {
        protected PlayerController _player;
        
        protected const float MOVE_THRESHOLD = 0.01f;

        public AbstractPlayerState(GameObject owner, StateSO stateData) : base(owner, stateData)
        {
            _player = owner.GetComponent<PlayerController>();
            Debug.Assert(_player != null, "PlayerController가 null입니다. 플레이어 상태는 반드시 플레이어의 자식이어야 합니다.");
            
        }

        public override void Enter()
        {
            if (_stateSO.animHash != null)
                _player.Renderer.RenderClipIfNotPlaying(_stateSO.animHash.HashValue);
        }
    }
}