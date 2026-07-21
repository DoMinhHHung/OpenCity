using UnityEngine;
using OpenCity.Player.FSM;
using OpenCity.Player.InputHandling;
using OpenCity.Player.CameraDirection;

namespace OpenCity.Tests
{
    public static class TestUtilities
    {
        public static PlayerLocomotionConfig CreateTestConfig()
        {
            var config = ScriptableObject.CreateInstance<PlayerLocomotionConfig>();
            return config;
        }

        public static GameObject CreateTestPlayerObject(string name = "TestPlayer")
        {
            var playerObj = new GameObject(name);
            var controller = playerObj.AddComponent<CharacterController>();
            
            controller.radius = 0.5f;
            controller.height = 2f;
            controller.center = new Vector3(0, 1f, 0);
            
            return playerObj;
        }

        public static bool ApproximatelyEqual(Vector3 a, Vector3 b, float tolerance = 0.001f)
        {
            return Vector3.Distance(a, b) < tolerance;
        }

        public static bool ApproximatelyEqual(Vector2 a, Vector2 b, float tolerance = 0.001f)
        {
            return Vector2.Distance(a, b) < tolerance;
        }

        public static bool IsNormalized(Vector3 v, float tolerance = 0.001f)
        {
            return Mathf.Abs(v.magnitude - 1f) < tolerance;
        }
    }

    #region Mock Implementations

    public class MockInputReader : IInputReader
    {
        private Vector2 _moveInput;
        private Vector2 _lookInput;
        private bool _jumpPressed;
        private bool _sprintHeld;
        private bool _dodgePressed;
        private bool _attackPressed;

        public Vector2 MoveInput => _moveInput;
        public Vector2 LookInput => _lookInput;
        public bool JumpPressed => _jumpPressed;
        public bool SprintHeld => _sprintHeld;
        public bool DodgePressed => _dodgePressed;
        public bool AttackPressed => _attackPressed;

        public void SetMoveInput(Vector2 input) => _moveInput = input;
        public void SetLookInput(Vector2 input) => _lookInput = input;
        public void SimulateJumpPress() => _jumpPressed = true;
        public void SimulateSprintHold(bool held) => _sprintHeld = held;
        public void SimulateDodgePress() => _dodgePressed = true;
        public void SimulateAttackPress() => _attackPressed = true;
        
        public void ResetButtons()
        {
            _jumpPressed = false;
            _dodgePressed = false;
            _attackPressed = false;
        }
    }

    public class MockCameraDirectionProvider : ICameraDirectionProvider
    {
        private Vector3 _forward = Vector3.forward;
        private Vector3 _right = Vector3.right;

        public Vector3 Forward => _forward;
        public Vector3 Right => _right;

        public void SetForward(Vector3 forward)
        {
            _forward = forward.normalized;
            _right = Vector3.Cross(Vector3.up, _forward).normalized;
        }

        public void SetRight(Vector3 right)
        {
            _right = right.normalized;
            _forward = Vector3.Cross(_right, Vector3.up).normalized;
        }
    }

    public class MockPlayerStateMachine : IPlayerStateMachine
    {
        public int ChangeStateCallCount { get; private set; }
        public System.Type LastRequestedStateType { get; private set; }

        public void ChangeState<TState>() where TState : IPlayerState
        {
            ChangeStateCallCount++;
            LastRequestedStateType = typeof(TState);
        }

        public void Reset()
        {
            ChangeStateCallCount = 0;
            LastRequestedStateType = null;
        }
    }

    #endregion
}
