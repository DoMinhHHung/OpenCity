namespace OpenCity.Player.FSM
{
    /// <summary>
    /// Contract for a single locomotion state in the Player finite state machine.
    /// Each implementation owns its own transition logic and decides
    /// independently when to move to another state.
    /// </summary>
    public interface IPlayerState
    {
        void Enter();
        void Tick(float deltaTime);
        void Exit();
    }
}
