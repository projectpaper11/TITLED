using System.Threading;
using System.Threading.Tasks;
using DevLib.ServiceLocator;
using DevLib.SoundSystem.Runtime;
using UnityEngine;

namespace GameSystem
{
    public class FrameMove : MonoBehaviour
    {
        [SerializeField] private GameObject targetFrame;
        [SerializeField] private GameObject noise;
        [SerializeField] private SoundClipSO noiseSFX;

        private CancellationTokenSource _cts;

        public void MoveFrame()
        {
            noise.SetActive(true);
            if (targetFrame)
                targetFrame.SetActive(true);
            else
                gameObject.SetActive(false);
            MoveFrameAsync();
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
        }

        private async void MoveFrameAsync()
        {
            ServiceLocator.Get<IAudioService>().PlaySfx(noiseSFX,1);
            _cts = new CancellationTokenSource();
            await Task.Delay(500,_cts.Token);
            noise.SetActive(false);
            ServiceLocator.Get<IAudioService>().StopSfx(1);
        }
    }
}