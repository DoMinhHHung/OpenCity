using UnityEngine;
using OpenCity.Player.InputHandling;
using OpenCity.Player.CameraDirection;

namespace OpenCity.Player.FSM
{
    /// <summary>
    /// Immutable bundle of everything a locomotion state needs. States
    /// receive exactly one PlayerContext via their constructor - no direct
    /// references to other states, no hidden dependencies.
    /// </summary>
    public class PlayerContext
    {
        public CharacterController Controller { get; }
        public Transform Transform { get; }
        public IInputReader Input { get; }
        public ICameraDirectionProvider CameraDirection { get; }
        public IPlayerStateMachine StateMachine { get; }

        public PlayerContext(
            CharacterController controller,
            Transform transform,
            IInputReader input,
            ICameraDirectionProvider cameraDirection,
            IPlayerStateMachine stateMachine)
        {
            Controller = controller;
            Transform = transform;
            Input = input;
            CameraDirection = cameraDirection;
            StateMachine = stateMachine;
        }
    }
}
