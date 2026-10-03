
using UnityEngine;

namespace JumpScareSystem
{
    public abstract class AbstractJumpScare : UnityEngine.MonoBehaviour
    {
        public abstract void ShowSpotEffect();

        public abstract void ShowJumpScare();
        public abstract void DeadEffect();
    }
}