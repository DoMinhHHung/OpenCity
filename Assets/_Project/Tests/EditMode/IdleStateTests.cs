using NUnit.Framework;
using UnityEngine;
using OpenCity.Player.FSM;
using OpenCity.Player.FSM.States;
using OpenCity.Player.InputHandling;
using OpenCity.Tests.Utilities;  

namespace OpenCity.Tests.Editor
{
    [TestFixture]
    public class IdleStateTests
    {
        private MockInputReader mockInput;
        private PlayerLocomotionConfig config;
        private IdleState idleState;

        [SetUp]
        public void SetUp()
        {
            mockInput = new MockInputReader();
            
            config = ScriptableObject.CreateInstance<PlayerLocomotionConfig>();
            
        }

        [Test]
        public void IdleState_ReceivesStrongMovementInput_CanTransition()
        {
            mockInput.MoveInput = new Vector2(1f, 0f);

            var go = new GameObject("TestPlayer");
            var controller = go.AddComponent<CharacterController>();

            var context = new PlayerContext(
                controller: controller,
                transform: go.transform,
                input: mockInput,
                cameraDirection: null,
                config: config,
                stateMachine: null
            );

            idleState = new IdleState(context);
            idleState.Tick(0.016f);

            Assert.Greater(mockInput.MoveInput.sqrMagnitude, 0.01f);
        }

        [Test]
        public void IdleState_SmallOrZeroInput_RemainsIdle()
        {
            mockInput.MoveInput = Vector2.zero;

            var go = new GameObject("TestPlayer");
            var controller = go.AddComponent<CharacterController>();

            var context = new PlayerContext(
                controller, go.transform, mockInput, null, config, null
            );

            idleState = new IdleState(context);
            idleState.Tick(0.016f);

            Assert.LessOrEqual(mockInput.MoveInput.sqrMagnitude, 0.01f);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(config);
        }
    }
}