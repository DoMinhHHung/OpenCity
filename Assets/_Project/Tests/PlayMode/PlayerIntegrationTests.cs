using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace OpenCity.Tests.PlayMode
{
    public class PlayerIntegrationTests
    {
        private GameObject playerObject;
        private Rigidbody playerRigidbody;

        [SetUp]
        public void SetUp()
        {
            playerObject = new GameObject("TestPlayer");
            playerObject.transform.position = new Vector3(0, 10, 0);
            
            playerRigidbody = playerObject.AddComponent<Rigidbody>();
            playerRigidbody.useGravity = true;
            playerRigidbody.mass = 1f;

            var collider = playerObject.AddComponent<CapsuleCollider>();
            collider.height = 2f;
            collider.radius = 0.5f;
        }

        [TearDown]
        public void TearDown()
        {
            if (playerObject != null)
            {
                Object.Destroy(playerObject);
            }
        }

        [UnityTest]
        public IEnumerator Player_AffectedByGravity_FallsDown()
        {
            float initialYPosition = playerObject.transform.position.y;

            yield return new WaitForSeconds(0.5f);

            Assert.Less(playerObject.transform.position.y, initialYPosition);
        }

        [UnityTest]
        public IEnumerator PlayerRigidbody_VelocityUpdates_WhenForceApplied()
        {
            Vector3 appliedForce = Vector3.forward * 10f;
            
            playerRigidbody.AddForce(appliedForce, ForceMode.Impulse);
            
            yield return new WaitForFixedUpdate();

            Assert.Greater(playerRigidbody.linearVelocity.z, 0f);
        }
    }
}