using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using OpenCity.Player.FSM;
using OpenCity.Player.InputHandling;
using OpenCity.Player.CameraDirection;
using VContainer;
using VContainer.Unity;

namespace OpenCity.Tests.PlayMode
{
    public class PlayerSystemPlayModeTests
    {
        private GameObject _testPlayerObject;
        private PlayerStateMachine _playerStateMachine;

        #region Setup & Teardown

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            _testPlayerObject = new GameObject("TestPlayer");
            _testPlayerObject.AddComponent<CharacterController>();
            
            yield return null;
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            if (_testPlayerObject != null)
            {
                Object.Destroy(_testPlayerObject);
            }
            
            yield return null; 
        }

        #endregion

        #region CharacterController Integration Tests

        [UnityTest]
        public IEnumerator CharacterController_ShouldBeAddedToPlayer()
        {
            var controller = _testPlayerObject.GetComponent<CharacterController>();
            Assert.IsNotNull(controller, "Player phải có CharacterController component");
            
            yield return null;
        }

        [UnityTest]
        public IEnumerator CharacterController_Move_ShouldChangePosition()
        {
            var controller = _testPlayerObject.GetComponent<CharacterController>();
            Vector3 startPosition = _testPlayerObject.transform.position;
            Vector3 moveDirection = Vector3.forward;

            for (int i = 0; i < 10; i++)
            {
                controller.Move(moveDirection * Time.deltaTime);
                yield return null;
            }

            Assert.AreNotEqual(startPosition, _testPlayerObject.transform.position,
                "Position phải thay đổi sau khi di chuyển");
            Assert.Greater(_testPlayerObject.transform.position.z, startPosition.z,
                "Player phải di chuyển về phía trước (Z+)");
        }

        #endregion

        #region PlayerLocomotionConfig Tests

        [UnityTest]
        public IEnumerator PlayerLocomotionConfig_CreateInstance_ShouldWork()
        {
            var config = ScriptableObject.CreateInstance<PlayerLocomotionConfig>();
            
            yield return null;

            Assert.IsNotNull(config);
            Assert.Greater(config.WalkSpeed, 0f);
            Assert.Greater(config.RotationSpeed, 0f);
            
            Object.Destroy(config);
        }

        #endregion

        #region Mock Input Reader Tests

        [UnityTest]
        public IEnumerator MockInputReader_ShouldProvideInputValues()
        {
            var mockInput = new MockInputReader();
            mockInput.SetMoveInput(new Vector2(1f, 0f));

            yield return null;

            Assert.AreEqual(new Vector2(1f, 0f), mockInput.MoveInput);
            Assert.IsFalse(mockInput.JumpPressed);
            Assert.IsFalse(mockInput.SprintHeld);
        }

        [UnityTest]
        public IEnumerator MockInputReader_ButtonPresses_ShouldWork()
        {
            var mockInput = new MockInputReader();

            mockInput.SimulateJumpPress();
            yield return null;

            Assert.IsTrue(mockInput.JumpPressed, "Jump pressed phải trả về true");
            
            yield return null;
            Assert.IsFalse(mockInput.JumpPressed, "Jump pressed phải reset về false");
        }

        #endregion

        #region Camera Direction Tests

        [UnityTest]
        public IEnumerator CameraDirectionProvider_ShouldProvideDirections()
        {
            var cameraProvider = new MockCameraDirectionProvider();

            yield return null;

            Assert.AreEqual(Vector3.forward, cameraProvider.Forward);
            Assert.AreEqual(Vector3.right, cameraProvider.Right);
        }

        [UnityTest]
        public IEnumerator CameraDirectionProvider_CustomDirection_ShouldWork()
        {
            var cameraProvider = new MockCameraDirectionProvider();
            Vector3 customForward = new Vector3(1f, 0f, 1f).normalized;
            
            cameraProvider.SetForward(customForward);
            yield return null;

            Assert.AreEqual(customForward, cameraProvider.Forward);
        }

        #endregion

        #region Physics & Gravity Tests

        [UnityTest]
        public IEnumerator Player_WithRigidbody_ShouldFallDown()
        {
            var rb = _testPlayerObject.AddComponent<Rigidbody>();
            _testPlayerObject.transform.position = new Vector3(0, 10, 0);
            float startY = _testPlayerObject.transform.position.y;

            yield return new WaitForSeconds(0.5f);

            Assert.Less(_testPlayerObject.transform.position.y, startY,
                "Player với Rigidbody phải rơi xuống do trọng lực");
        }

        #endregion

        #region Performance Tests

        [UnityTest]
        public IEnumerator PlayerSystem_ShouldMaintain60FPS()
        {
            int frameCount = 0;
            float totalTime = 0f;
            float targetTime = 1f; 

            while (totalTime < targetTime)
            {
                frameCount++;
                totalTime += Time.deltaTime;
                yield return null;
            }

            float averageFPS = frameCount / totalTime;
            Assert.GreaterOrEqual(averageFPS, 55f, 
                "Player system phải maintain ít nhất 55 FPS (gần 60 FPS)");
        }

        #endregion

        #region Helper Classes

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

        #endregion
    }
}
