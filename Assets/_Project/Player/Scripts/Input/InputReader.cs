using UnityEngine;
using UnityEngine.InputSystem;

namespace OpenCity.Player.InputHandling
{
    public class InputReader : IInputReader
    {
        private readonly InputAction _move;
        private readonly InputAction _look;
        private readonly InputAction _jump;
        private readonly InputAction _sprint;
        private readonly InputAction _dodge;
        private readonly InputAction _attack;

        public InputReader()
        {
            var onFoot = InputSystem.actions.FindActionMap("OnFoot", throwIfNotFound: true);
            onFoot.Enable();

            _move = onFoot.FindAction("Move", throwIfNotFound: true);
            _look = onFoot.FindAction("Look", throwIfNotFound: true);
            _jump = onFoot.FindAction("Jump", throwIfNotFound: true);
            _sprint = onFoot.FindAction("Sprint", throwIfNotFound: true);
            _dodge = onFoot.FindAction("Dodge", throwIfNotFound: true);
            _attack = onFoot.FindAction("Attack", throwIfNotFound: true);
        }

        public Vector2 MoveInput => _move.ReadValue<Vector2>();
        public Vector2 LookInput => _look.ReadValue<Vector2>();
        public bool JumpPressed => _jump.WasPressedThisFrame();
        public bool SprintHeld => _sprint.IsPressed();
        public bool DodgePressed => _dodge.WasPressedThisFrame();
        public bool AttackPressed => _attack.WasPressedThisFrame();
    }
}
