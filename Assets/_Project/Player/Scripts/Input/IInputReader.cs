using UnityEngine;

namespace OpenCity.Player.InputHandling
{
    /// <summary>
    /// Exposes player input as clean, already-processed values.
    /// Decouples gameplay logic from the Input System package's generated API,
    /// so states never reference InputAction or PlayerInput directly.
    /// </summary>
    public interface IInputReader
    {
        Vector2 MoveInput { get; }
        Vector2 LookInput { get; }
        bool JumpPressed { get; }
        bool SprintHeld { get; }
        bool DodgePressed { get; }
        bool AttackPressed { get; }
    }
}
