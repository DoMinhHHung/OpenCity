using System;
using UnityEngine;

namespace OpenCity.Player.InputHandling
{
    /// <summary>
    /// Adapter over the generated Input System class. This is the only
    /// place in the codebase allowed to reference OpenCityInputActions -
    /// everything else depends on IInputReader.
    /// </summary>
    public class InputReader : IInputReader, IDisposable
    {
        private readonly OpenCityInputActions _actions;
        private readonly OpenCityInputActions.OnFootActions _onFoot;

        public InputReader()
        {
            _actions = new OpenCityInputActions();
            _onFoot = _actions.OnFoot;
            _onFoot.Enable();
        }

        public Vector2 MoveInput => _onFoot.Move.ReadValue<Vector2>();
        public Vector2 LookInput => _onFoot.Look.ReadValue<Vector2>();
        public bool JumpPressed => _onFoot.Jump.WasPressedThisFrame();
        public bool SprintHeld => _onFoot.Sprint.IsPressed();
        public bool DodgePressed => _onFoot.Dodge.WasPressedThisFrame();
        public bool AttackPressed => _onFoot.Attack.WasPressedThisFrame();

        public void Dispose()
        {
            _onFoot.Disable();
            _actions.Dispose();
        }
    }
}
