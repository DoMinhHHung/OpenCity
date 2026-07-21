using UnityEngine;

namespace OpenCity.Player.CameraDirection
{
    /// <summary>
    /// Binding used until the Camera system exists. Forward/Right map
    /// directly to world axes instead of camera axes. Swapping to a
    /// camera-based implementation later only touches this class and its
    /// registration in PlayerLifetimeScope - no state code changes.
    /// </summary>
    public class WorldAxisCameraDirectionProvider : ICameraDirectionProvider
    {
        public Vector3 Forward => Vector3.forward;
        public Vector3 Right => Vector3.right;
    }
}
