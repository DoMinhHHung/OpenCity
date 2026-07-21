namespace OpenCity.Player.FSM
{
    /// <summary>
    /// Minimal surface exposed to states. A state may only request a
    /// transition - it has no visibility into registration or lifecycle.
    /// </summary>
    public interface IPlayerStateMachine
    {
        void ChangeState<TState>() where TState : IPlayerState;
    }
}
