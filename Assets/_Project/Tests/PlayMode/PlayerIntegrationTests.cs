using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using OpenCity.Player.FSM;
using OpenCity.Tests.Utilities;

namespace OpenCity.Tests.PlayMode
{
    public class PlayerIntegrationTests
    {
        private GameObject playerObject;
        private PlayerStateMachine stateMachine;
        private MockInputReader mockInput;
        private PlayerLocomotionConfig config;

        [SetUp]
        public void SetUp()
        {
            playerObject = new GameObject("TestPlayer");
            playerObject.transform.position = new Vector3(0f, 10f, 0f);
            playerObject.AddComponent<CharacterController>();

            mockInput = new MockInputReader();
            config = ScriptableObject.CreateInstance<PlayerLocomotionConfig>();

            stateMachine = playerObject.AddComponent<PlayerStateMachine>();
            stateMachine.Construct(mockInput, new MockCameraDirectionProvider(), config);
        }

        [TearDown]
        public void TearDown()
        {
            if (config != null) Object.DestroyImmediate(config);
            if (playerObject != null) Object.Destroy(playerObject);
        }

        [UnityTest]
        public IEnumerator Player_Airborne_FallsUnderGravity()
        {
            float initialY = playerObject.transform.position.y;

            yield return new WaitForSeconds(0.5f);

            Assert.Less(playerObject.transform.position.y, initialY);
        }

        [UnityTest]
        public IEnumerator Player_WithForwardInput_TransitionsToWalkAndMovesHorizontally()
        {
            Vector3 initialPosition = playerObject.transform.position;
            mockInput.MoveInput = new Vector2(0f, 1f);

            yield return new WaitForSeconds(0.3f);

            Vector3 delta = playerObject.transform.position - initialPosition;
            delta.y = 0f;
            Assert.Greater(delta.magnitude, 0.01f);
        }

        [UnityTest]
        public IEnumerator Player_WithoutInput_StaysIdleAndDoesNotMoveHorizontally()
        {
            Vector3 initialPosition = playerObject.transform.position;
            mockInput.MoveInput = Vector2.zero;

            yield return new WaitForSeconds(0.3f);

            Vector3 delta = playerObject.transform.position - initialPosition;
            delta.y = 0f;
            Assert.AreEqual(0f, delta.magnitude, 0.001f);
        }
    }
}