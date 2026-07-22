namespace OpenCity.Player.FSM.States
{
    /// <summary>
    /// Default locomotion state. Player starts here on spawn and returns
    /// here whenever there is no movement input.
    /// </summary>
    public class IdleState : IPlayerState
    {
        private readonly PlayerContext _context;

        public IdleState(PlayerContext context)
        {
            _context = context;
        }

        public void Enter()
        {
            // Chưa có Animator - sẽ nối animation trigger khi hệ thống animation tồn tại
        }

        public void Tick(float deltaTime)
        {
            float deadzone = _context.Config.MoveInputDeadzone;
            float sqrMagnitude = _context.Input.MoveInput.sqrMagnitude;
            if (sqrMagnitude > deadzone * deadzone)
            {
                _context.StateMachine.ChangeState<WalkState>();
            }
        }

        public void Exit()
        {
        }
    }
}
