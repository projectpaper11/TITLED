using DevLib.ModuleSystem;
using UnityEngine;

namespace Agents
{
    public interface ISpotLightModule
    {
        void SetSpotLightFacing(Vector3 dir);
        void SetSpotLightImmediatly(Vector3 dir);
    }

    public class SpotLightModule : MonoModule, ISpotLightModule
    {
        [SerializeField] private float sensitivity = 0.1f;
        
        public void SetSpotLightFacing(Vector3 dir)
        {
            if(Mathf.Approximately(dir.magnitude, 0))
                return;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(0f, 0f, angle + 90f),sensitivity * Time.deltaTime);
        }

        public void SetSpotLightImmediatly(Vector3 dir)
        {
            if (Mathf.Approximately(dir.magnitude, 0))
                return;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
        }
    }
}