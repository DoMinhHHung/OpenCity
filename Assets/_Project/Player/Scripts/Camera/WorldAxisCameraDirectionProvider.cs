using UnityEngine;

namespace OpenCity.Player.CameraDirection
{
    public class WorldAxisCameraDirectionProvider : ICameraDirectionProvider
    {
        public Vector3 Forward => Vector3.forward;
        public Vector3 Right => Vector3.right;
    }
}
