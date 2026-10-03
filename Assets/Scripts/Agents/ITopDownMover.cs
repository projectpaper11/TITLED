using System;
using UnityEngine;

namespace Agents
{
    public interface ITopDownMover
    {
        event Action<Vector2> OnMovementChanged; //방향변경시의 이벤트
        void SetMovementSpeed(float speed); //속도 설정
        void SetMovement(Vector2 movement); //방향설정
        void StopImmediately(); //정지
    }
}