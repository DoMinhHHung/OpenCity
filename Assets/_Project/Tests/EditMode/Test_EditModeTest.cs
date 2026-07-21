using NUnit.Framework;
using UnityEngine;
using OpenCity.Player.FSM;
using OpenCity.Player.FSM.States;
using OpenCity.Player.InputHandling;
using OpenCity.Player.CameraDirection;

namespace OpenCity.Tests.Editor
{
    /// <summary>
    /// EditMode tests cho Player System.
    /// Tests này kiểm tra logic không cần GameObject/Scene thực tế.
    /// </summary>
    public class PlayerSystemEditModeTests
    {
        #region PlayerContext Tests

        [Test]
        public void PlayerContext_Constructor_ShouldInitializeAllProperties()
        {
            // Arrange
            var mockController = new GameObject("TestController").AddComponent<CharacterController>();
            var mockTransform = new GameObject("TestPlayer").transform;
            var mockInput = new MockInputReader();
            var mockCamera = new MockCameraDirectionProvider();
            var mockStateMachine = new MockPlayerStateMachine();
            var config = ScriptableObject.CreateInstance<PlayerLocomotionConfig>();

            // Act
            var context = new PlayerContext(
                mockController,
                mockTransform,
                mockInput,
                mockCamera,
                config,
                mockStateMachine
            );

            // Assert
            Assert.AreEqual(mockController, context.Controller);
            Assert.AreEqual(mockTransform, context.Transform);
            Assert.AreEqual(mockInput, context.Input);
            Assert.AreEqual(mockCamera, context.CameraDirection);
            Assert.AreEqual(config, context.Config);
            Assert.AreEqual(mockStateMachine, context.StateMachine);

            // Cleanup
            Object.DestroyImmediate(mockController.gameObject);
            Object.DestroyImmediate(mockTransform.gameObject);
            Object.DestroyImmediate(config);
        }

        #endregion

        #region PlayerLocomotionConfig Tests

        [Test]
        public void PlayerLocomotionConfig_DefaultValues_ShouldBeReasonable()
        {
            // Arrange & Act
            var config = ScriptableObject.CreateInstance<PlayerLocomotionConfig>();

            // Assert
            Assert.Greater(config.WalkSpeed, 0f, "Walk speed phải lớn hơn 0");
            Assert.Greater(config.RotationSpeed, 0f, "Rotation speed phải lớn hơn 0");
            Assert.That(config.MoveInputDeadzone, Is.InRange(0f, 1f), 
                "Move input deadzone phải nằm trong khoảng 0-1");

            // Cleanup
            Object.DestroyImmediate(config);
        }

        #endregion

        #region IdleState Tests

        [Test]
        public void IdleState_Enter_ShouldNotThrowException()
        {
            // Arrange
            var context = CreateMockPlayerContext();
            var idleState = new IdleState(context.context);

            // Act & Assert
            Assert.DoesNotThrow(() => idleState.Enter());

            // Cleanup
            CleanupMockContext(context);
        }

        [Test]
        public void IdleState_Exit_ShouldNotThrowException()
        {
            // Arrange
            var context = CreateMockPlayerContext();
            var idleState = new IdleState(context.context);

            // Act & Assert
            Assert.DoesNotThrow(() => idleState.Exit());

            // Cleanup
            CleanupMockContext(context);
        }

        [Test]
        public void IdleState_Tick_WithNoInput_ShouldRemainInIdle()
        {
            // Arrange
            var mockContext = CreateMockPlayerContext();
            mockContext.mockInput.SetMoveInput(Vector2.zero);
            var idleState = new IdleState(mockContext.context);

            // Act
            idleState.Tick(0.016f); // Simulate one frame at 60fps

            // Assert
            // State machine không được gọi ChangeState (check qua mock)
            Assert.AreEqual(0, mockContext.mockStateMachine.ChangeStateCallCount,
                "IdleState không nên chuyển state khi không có input");

            // Cleanup
            CleanupMockContext(mockContext);
        }

        [Test]
        public void IdleState_Tick_WithInputBelowDeadzone_ShouldRemainInIdle()
        {
            // Arrange
            var mockContext = CreateMockPlayerContext();
            // Input nhỏ hơn deadzone (0.1)
            mockContext.mockInput.SetMoveInput(new Vector2(0.05f, 0.05f));
            var idleState = new IdleState(mockContext.context);

            // Act
            idleState.Tick(0.016f);

            // Assert
            Assert.AreEqual(0, mockContext.mockStateMachine.ChangeStateCallCount,
                "IdleState không nên chuyển state khi input dưới deadzone");

            // Cleanup
            CleanupMockContext(mockContext);
        }

        [Test]
        public void IdleState_Tick_WithInputAboveDeadzone_ShouldNotCrash()
        {
            // Arrange
            var mockContext = CreateMockPlayerContext();
            // Input lớn hơn deadzone
            mockContext.mockInput.SetMoveInput(new Vector2(0.5f, 0.5f));
            var idleState = new IdleState(mockContext.context);

            // Act & Assert
            // Hiện tại WalkState chưa được implement nên sẽ không crash
            Assert.DoesNotThrow(() => idleState.Tick(0.016f));

            // Cleanup
            CleanupMockContext(mockContext);
        }

        #endregion

        #region Vector Math Tests

        [Test]
        public void VectorMagnitude_CalculationTest()
        {
            // Test để verify logic sqrMagnitude trong IdleState
            Vector2 input = new Vector2(0.1f, 0f);
            float deadzone = 0.1f;
            
            float sqrMag = input.sqrMagnitude;
            float deadzoneSquared = deadzone * deadzone;
            
            Assert.AreEqual(0.01f, sqrMag, 0.0001f);
            Assert.AreEqual(0.01f, deadzoneSquared, 0.0001f);
            Assert.IsFalse(sqrMag > deadzoneSquared, 
                "Input 0.1 không nên vượt qua deadzone 0.1");
        }

        #endregion

        #region Helper Methods

        private (PlayerContext context, 
                 MockInputReader mockInput, 
                 MockCameraDirectionProvider mockCamera, 
                 MockPlayerStateMachine mockStateMachine,
                 PlayerLocomotionConfig config,
                 Transform transform,
                 GameObject controllerObj) CreateMockPlayerContext()
        {
            var controllerObj = new GameObject("TestController");
            var mockController = controllerObj.AddComponent<CharacterController>();
            var transform = new GameObject("TestPlayer").transform;
            var mockInput = new MockInputReader();
            var mockCamera = new MockCameraDirectionProvider();
            var mockStateMachine = new MockPlayerStateMachine();
            var config = ScriptableObject.CreateInstance<PlayerLocomotionConfig>();

            mockCamera.SetForward(Vector3.forward);

            var context = new PlayerContext(
                mockController,
                transform,
                mockInput,
                mockCamera,
                config,
                mockStateMachine
            );

            return (context, mockInput, mockCamera, mockStateMachine, config, transform, controllerObj);
        }

        private void CleanupMockContext((PlayerContext context, 
                                         MockInputReader mockInput, 
                                         MockCameraDirectionProvider mockCamera, 
                                         MockPlayerStateMachine mockStateMachine,
                                         PlayerLocomotionConfig config,
                                         Transform transform,
                                         GameObject controllerObj) mockContext)
        {
            if (mockContext.transform != null)
                Object.DestroyImmediate(mockContext.transform.gameObject);
            if (mockContext.controllerObj != null)
                Object.DestroyImmediate(mockContext.controllerObj);
            if (mockContext.config != null)
                Object.DestroyImmediate(mockContext.config);
        }

        #endregion

        #region Mock Classes

        /// <summary>
        /// Mock implementation của IInputReader cho testing
        /// </summary>
        private class MockInputReader : IInputReader
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
            public void SetJumpPressed(bool pressed) => _jumpPressed = pressed;
            public void SetSprintHeld(bool held) => _sprintHeld = held;
            public void SetDodgePressed(bool pressed) => _dodgePressed = pressed;
            public void SetAttackPressed(bool pressed) => _attackPressed = pressed;
        }

        /// <summary>
        /// Mock implementation của ICameraDirectionProvider
        /// </summary>
        private class MockCameraDirectionProvider : ICameraDirectionProvider
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
        }

        /// <summary>
        /// Mock implementation của IPlayerStateMachine
        /// </summary>
        private class MockPlayerStateMachine : IPlayerStateMachine
        {
            public int ChangeStateCallCount { get; private set; }
            public System.Type LastRequestedStateType { get; private set; }

            public void ChangeState<TState>() where TState : IPlayerState
            {
                ChangeStateCallCount++;
                LastRequestedStateType = typeof(TState);
            }
        }

        #endregion
    }
}
