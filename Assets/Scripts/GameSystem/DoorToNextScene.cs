using DevLib.ServiceLocator;
using UnityEngine;

namespace GameSystem
{
    public class DoorToNextScene : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D coll)
        {
            coll.enabled = false;
            ServiceLocator.Get<ISceneService>().MoveSceneWithNoise(2);
        }
    }
}