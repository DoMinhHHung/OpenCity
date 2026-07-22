using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using OpenCity.Player.FSM;
using OpenCity.Tests.Utilities;

namespace OpenCity.Tests.Editor
{
    [TestFixture]
    public class PlayerStateMachineTests
    {
        private GameObject go;
        private PlayerStateMachine stateMachine;
        private PlayerLocomotionConfig config;
        private MockPlayerState stateA;
        private OtherMockState stateB;

        [SetUp]
        public void SetUp()
        {
            go = new GameObject("TestPlayer");
            go.AddComponent<CharacterController>();
            stateMachine = go.AddComponent<PlayerStateMachine>();

            config = ScriptableObject.CreateInstance<PlayerLocomotionConfig>();
            stateMachine.Construct(new MockInputReader(), new MockCameraDirectionProvider(), config);

            stateA = new MockPlayerState();
            stateB = new OtherMockState();
            stateMachine.RegisterState(stateA);
            stateMachine.RegisterState(stateB);
        }

        [Test]
        public void SetInitialState_EntersRegisteredState()
        {
            stateMachine.SetInitialState<MockPlayerState>();

            Assert.AreEqual(1, stateA.EnterCallCount);
        }

        [Test]
        public void ChangeState_ExitsPreviousAndEntersNext()
        {
            stateMachine.SetInitialState<MockPlayerState>();

            stateMachine.ChangeState<OtherMockState>();

            Assert.AreEqual(1, stateA.ExitCallCount);
            Assert.IsTrue(stateB.HasEntered);
        }

        [Test]
        public void ChangeState_RequestingCurrentStateType_IsNoOp()
        {
            stateMachine.SetInitialState<MockPlayerState>();

            stateMachine.ChangeState<MockPlayerState>();

            Assert.AreEqual(1, stateA.EnterCallCount);
            Assert.AreEqual(0, stateA.ExitCallCount);
        }

        [Test]
        public void ChangeState_UnregisteredStateType_LogsErrorAndKeepsCurrentState()
        {
            stateMachine.SetInitialState<MockPlayerState>();

            LogAssert.Expect(LogType.Error,
                $"[PlayerStateMachine] Cannot change state - {nameof(UnregisteredMockState)} is not registered.");
            stateMachine.ChangeState<UnregisteredMockState>();

            Assert.AreEqual(0, stateA.ExitCallCount);
        }

        [TearDown]
        public void TearDown()
        {
            if (config != null) Object.DestroyImmediate(config);
            if (go != null) Object.DestroyImmediate(go);
        }

        private class OtherMockState : IPlayerState
        {
            public bool HasEntered { get; private set; }
            public void Enter() => HasEntered = true;
            public void Tick(float deltaTime) { }
            public void Exit() { }
        }

        private class UnregisteredMockState : IPlayerState
        {
            public void Enter() { }
            public void Tick(float deltaTime) { }
            public void Exit() { }
        }
    }
}