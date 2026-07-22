using UnityEngine;

namespace OpenCity.Player.CameraDirection
{
    /// <summary>
    /// Reads horizontal forward/right axes from the active output Camera,
    /// which CinemachineBrain drives every frame regardless of which
    /// CinemachineCamera is currently blended in.
    /// </summary>
    public class MainCameraDirectionProvider : ICameraDirectionProvider
    {
        private readonly Transform _cameraTransform;

        public MainCameraDirectionProvider(Transform cameraTransform)
        {
            _cameraTransform = cameraTransform;
        }

        public Vector3 Forward
        {
            get
            {
                Vector3 forward = _cameraTransform.forward;
                forward.y = 0f;
                return forward.normalized;
            }
        }

        public Vector3 Right
        {
            get
            {
                Vector3 right = _cameraTransform.right;
                right.y = 0f;
                return right.normalized;
            }
        }
    }
}
