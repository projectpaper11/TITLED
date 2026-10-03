using System;
using System.Threading;
using System.Threading.Tasks;
using DevLib.ServiceLocator;
using Febucci.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameSystem
{
    public class EndingScriptSystem : MonoBehaviour
    {
        [SerializeField] private TypewriterByCharacter writer;
        private CancellationTokenSource _cts;

        private void Start()
        {
            _cts = new CancellationTokenSource();
            WriteScriptAsync();
        }

        private async void WriteScriptAsync()
        {
            await Task.Delay(500,_cts.Token);
            writer.ShowText("수고하셨습니다!");
            await Task.Delay(2000,_cts.Token);
            writer.ShowText("오..");
            await Task.Delay(1200,_cts.Token);
            writer.ShowText("흥미롭군요.");
            await Task.Delay(2000,_cts.Token);
            writer.ShowText("다음 테스트로 넘어가겠습니다.");
            await Task.Delay(2000,_cts.Token);
            writer.ShowText("[테스트명 : UNTITLED]");
            await Task.Delay(2000,_cts.Token);
            ServiceLocator.Get<ISceneService>().MoveSceneWithNoise(0);
        }
    }
}