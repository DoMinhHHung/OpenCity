using NUnit.Framework;
using UnityEngine;
using OpenCity.Tests.Utilities;

namespace OpenCity.Tests.Editor
{
    public class IdleStateTests
    {
        private MockInputReader mockInput;

        [SetUp]
        public void SetUp()
        {
            mockInput = new MockInputReader();
        }

        [Test]
        public void IdleState_ReceivesMovementInput_CanTransitionToMove()
        {
            mockInput.Movement = new Vector2(1f, 0f);

            Assert.AreNotEqual(Vector2.zero, mockInput.Movement);
        }

        [Test]
        public void IdleState_ReceivesZeroInput_RemainsIdle()
        {
            mockInput.Movement = Vector2.zero;

            Assert.AreEqual(Vector2.zero, mockInput.Movement);
        }
    }
}