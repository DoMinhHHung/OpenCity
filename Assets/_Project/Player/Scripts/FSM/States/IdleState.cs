namespace OpenCity.Player.FSM.States
{
    /// <summary>
    /// Default locomotion state. Player starts here on spawn and returns
    /// here whenever there is no movement input.
    /// </summary>
    public class IdleState : IPlayerState
    {
        private const float MoveInputThreshold = 0.1f;

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
            float sqrMagnitude = _context.Input.MoveInput.sqrMagnitude;
            if (sqrMagnitude > MoveInputThreshold * MoveInputThreshold)
            {
                // TODO(walk-state): bật lại khi WalkState tồn tại
                // _context.StateMachine.ChangeState<WalkState>();
            }
        }

        public void Exit()
        {
        }
    }
}
