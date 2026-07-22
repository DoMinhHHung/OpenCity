using NUnit.Framework;
using UnityEngine;
using OpenCity.Player.FSM;
using OpenCity.Player.FSM.States;
using OpenCity.Tests.Utilities;

namespace OpenCity.Tests.Editor
{
    [TestFixture]
    public class IdleStateTests
    {
        private MockInputReader mockInput;
        private SpyStateMachine spyStateMachine;
        private PlayerLocomotionConfig config;
        private GameObject go;
        private PlayerContext context;
        private IdleState idleState;

        [SetUp]
        public void SetUp()
        {
            mockInput = new MockInputReader();
            spyStateMachine = new SpyStateMachine();
            config = ScriptableObject.CreateInstance<PlayerLocomotionConfig>();

            go = new GameObject("TestPlayer");
            var controller = go.AddComponent<CharacterController>();

            context = new PlayerContext(
                controller: controller,
                transform: go.transform,
                input: mockInput,
                cameraDirection: null,
                config: config,
                stateMachine: spyStateMachine
            );

            idleState = new IdleState(context);
        }

        [Test]
        public void Tick_MoveInputAboveDeadzone_RequestsTransitionToWalkState()
        {
            mockInput.MoveInput = new Vector2(1f, 0f);

            idleState.Tick(0.016f);

            Assert.AreEqual(1, spyStateMachine.ChangeStateCallCount);
            Assert.AreEqual(typeof(WalkState), spyStateMachine.LastRequestedStateType);
        }

        [Test]
        public void Tick_MoveInputAtOrBelowDeadzone_DoesNotRequestTransition()
        {
            mockInput.MoveInput = Vector2.zero;

            idleState.Tick(0.016f);

            Assert.AreEqual(0, spyStateMachine.ChangeStateCallCount);
        }

        [Test]
        public void Enter_ResetsHorizontalVelocityToZero()
        {
            context.Motion.HorizontalVelocity = new Vector3(5f, 0f, 3f);

            idleState.Enter();

            Assert.AreEqual(Vector3.zero, context.Motion.HorizontalVelocity);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(config);
            Object.DestroyImmediate(go);
        }
    }
}