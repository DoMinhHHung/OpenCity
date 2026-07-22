using UnityEngine;

namespace OpenCity.Player.CameraDirection
{
    public interface ICameraDirectionProvider
    {
        Vector3 Forward { get; }
        Vector3 Right { get; }
    }
}
