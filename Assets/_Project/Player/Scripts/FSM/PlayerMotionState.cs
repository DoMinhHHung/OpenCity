using UnityEngine;

namespace OpenCity.Player.FSM
{
    /// <summary>
    /// Mutable per-frame motion data shared by every locomotion state.
    /// Kept separate from PlayerContext to preserve the distinction between
    /// fixed dependencies (Context) and changing runtime data (this).
    /// </summary>
    public class PlayerMotionState
    {
        public Vector3 HorizontalVelocity { get; set; }
        public float VerticalVelocity { get; set; }
    }
}
