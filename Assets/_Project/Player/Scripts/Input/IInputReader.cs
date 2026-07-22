using UnityEngine;

namespace OpenCity.Player.InputHandling
{
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
