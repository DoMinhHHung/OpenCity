using UnityEngine;

namespace OpenCity.Player.FSM
{
    [CreateAssetMenu(fileName = "PlayerLocomotionConfig", menuName = "OpenCity/Player/Locomotion Config")]
    public class PlayerLocomotionConfig : ScriptableObject
    {
        [Header("Movement")]
        [SerializeField] private float walkSpeed = 3.5f;
        [SerializeField] private float rotationSpeed = 10f;

        [Header("Input")]
        [SerializeField, Range(0f, 1f)] private float moveInputDeadzone = 0.1f;

        public float WalkSpeed => walkSpeed;
        public float RotationSpeed => rotationSpeed;
        public float MoveInputDeadzone => moveInputDeadzone;
    }
}
