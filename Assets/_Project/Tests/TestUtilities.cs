using UnityEngine;

namespace OpenCity.Tests.Utilities
{
    public class MockInputReader : OpenCity.Player.InputHandling.IInputReader
    {
        public Vector2 MoveInput { get; set; }
        public Vector2 LookInput { get; set; }
        public bool JumpPressed { get; set; }
        public bool SprintHeld { get; set; }
        public bool DodgePressed { get; set; }
        public bool AttackPressed { get; set; }
    }

    public class MockCameraDirectionProvider 
    {
        public Vector3 ForwardDirection { get; set; } = Vector3.forward;
        public Vector3 RightDirection { get; set; } = Vector3.right;

        public Vector3 GetForward() => ForwardDirection;
        public Vector3 GetRight() => RightDirection;
    }

    public class MockPlayerState 
    {
        public bool HasEntered { get; private set; }
        public bool HasExited { get; private set; }
        public int TickCount { get; private set; }
        public int PhysicsTickCount { get; private set; }

        public void Enter() => HasEntered = true;
        public void Exit() => HasExited = true;
        public void Tick() => TickCount++;
        public void PhysicsTick() => PhysicsTickCount++;
    }
}