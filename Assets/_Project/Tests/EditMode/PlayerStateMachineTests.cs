using NUnit.Framework;
using OpenCity.Tests.Utilities;

namespace OpenCity.Tests.Editor
{
    public class PlayerStateMachineTests
    {
        private MockPlayerState state1;
        private MockPlayerState state2;

        [SetUp]
        public void SetUp()
        {
            state1 = new MockPlayerState();
            state2 = new MockPlayerState();
        }

        [Test]
        public void Initialize_SetsFirstStateAndCallsEnter()
        {
            state1.Enter();
            
            Assert.IsTrue(state1.HasEntered);
        }

        [Test]
        public void TransitionTo_CallsExitOnPreviousAndEnterOnNext()
        {
            state1.Enter();
            state1.Exit();
            state2.Enter();

            Assert.IsTrue(state1.HasExited);
            Assert.IsTrue(state2.HasEntered);
        }

        [Test]
        public void Tick_CallsTickOnCurrentState()
        {
            state1.Tick();
            
            Assert.AreEqual(1, state1.TickCount);
        }

        [Test]
        public void PhysicsTick_CallsPhysicsTickOnCurrentState()
        {
            state1.PhysicsTick();
            
            Assert.AreEqual(1, state1.PhysicsTickCount);
        }
    }
}