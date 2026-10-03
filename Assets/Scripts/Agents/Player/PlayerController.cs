using Agents.Player.FSM;
using DevLib.FsmSystem.Runtime;
using GameSystem;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

namespace Agents.Player
{
    public class PlayerController : AbstractAgent, IChangeLight
    {
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }
        [SerializeField] private StateListSO playerStateList;
        
        private StateMachine _stateMachine;
        public ISpotLightModule SpotLight { get; private set; }
         
        protected override void InitializeModules()
        {
            base.InitializeModules();
            PlayerInput.SetEnable();
            _stateMachine = new StateMachine(gameObject, playerStateList.states);
            SpotLight = GetModule<ISpotLightModule>();
            Debug.Assert(SpotLight != null, "SpotLight is null." );
        }

        private void Start()
        {
            ChangeState(PlayerState.IDLE);
        }

        private void OnDestroy()
        {
            PlayerInput.SetDisable();
        }
        
        private void Update()
        {
            _stateMachine?.UpdateMachine();
        }
        
        public void ChangeState(PlayerState newState)=> _stateMachine?.ChangeState((int)newState);
        public void ChangeSpotLight(Vector3 dir)
        {
            SpotLight.SetSpotLightImmediatly(dir);
        }

    }
}