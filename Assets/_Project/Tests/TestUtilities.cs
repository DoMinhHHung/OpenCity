using System;
using UnityEngine;
using OpenCity.Player.FSM;
using OpenCity.Player.InputHandling;
using OpenCity.Player.CameraDirection;

namespace OpenCity.Tests.Utilities
{
    public class MockInputReader : IInputReader
    {
        public Vector2 MoveInput { get; set; }
        public Vector2 LookInput { get; set; }
        public bool JumpPressed { get; set; }
        public bool SprintHeld { get; set; }
        public bool DodgePressed { get; set; }
        public bool AttackPressed { get; set; }
    }

    public class MockCameraDirectionProvider : ICameraDirectionProvider
    {
        public Vector3 Forward { get; set; } = Vector3.forward;
        public Vector3 Right { get; set; } = Vector3.right;
    }

    public class MockPlayerState : IPlayerState
    {
        public int EnterCallCount { get; private set; }
        public int ExitCallCount { get; private set; }
        public int TickCallCount { get; private set; }

        public bool HasEntered => EnterCallCount > 0;
        public bool HasExited => ExitCallCount > 0;

        public void Enter() => EnterCallCount++;
        public void Tick(float deltaTime) => TickCallCount++;
        public void Exit() => ExitCallCount++;
    }

    public class SpyStateMachine : IPlayerStateMachine
    {
        public int ChangeStateCallCount { get; private set; }
        public Type LastRequestedStateType { get; private set; }

        public void ChangeState<TState>() where TState : IPlayerState
        {
            ChangeStateCallCount++;
            LastRequestedStateType = typeof(TState);
        }
    }
}