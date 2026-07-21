using UnityEngine;

namespace OpenCity.Player.CameraDirection
{
    /// <summary>
    /// Supplies the horizontal forward/right axes that locomotion states use
    /// to convert raw input into camera-relative world-space movement.
    /// </summary>
    public interface ICameraDirectionProvider
    {
        Vector3 Forward { get; }
        Vector3 Right { get; }
    }
}
