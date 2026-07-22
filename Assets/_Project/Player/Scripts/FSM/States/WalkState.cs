using UnityEngine;

namespace OpenCity.Player.FSM.States
{
    /// <summary>
    /// Camera-relative horizontal locomotion. Transitions back to Idle
    /// once move input drops below the configured deadzone.
    /// Gravity is intentionally out of scope here - handled centrally by
    /// PlayerStateMachine so every state benefits without duplication.
    /// </summary>
    public class WalkState : IPlayerState
    {
        private readonly PlayerContext _context;

        public WalkState(PlayerContext context)
        {
            _context = context;
        }

        public void Enter()
        {
            // Chưa có Animator - sẽ nối animation trigger khi hệ thống animation tồn tại
        }

        public void Tick(float deltaTime)
        {
            Vector2 moveInput = _context.Input.MoveInput;
            float deadzone = _context.Config.MoveInputDeadzone;

            if (moveInput.sqrMagnitude <= deadzone * deadzone)
            {
                _context.StateMachine.ChangeState<IdleState>();
                return;
            }

            Vector3 moveDirection = _context.CameraDirection.Forward * moveInput.y
                                   + _context.CameraDirection.Right * moveInput.x;
            moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

            _context.Controller.Move(moveDirection * (_context.Config.WalkSpeed * deltaTime));

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            _context.Transform.rotation = Quaternion.Slerp(
                _context.Transform.rotation, targetRotation, _context.Config.RotationSpeed * deltaTime);
        }

        public void Exit()
        {
        }
    }
}
