using Cinemachine;
using UnityEngine;

namespace CustomCamera
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance;

        private CinemachineVirtualCamera _lastCamera;
        private const int MaxPriority = 999;
        private const int MinPriority = 555;

        private void Awake()
        {
            if (Instance is not null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
        
        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public void SetCamera(CinemachineVirtualCamera newCamera)
        {
            if (_lastCamera is not null) _lastCamera.Priority = MinPriority;
            newCamera.Priority = MaxPriority;
            _lastCamera = newCamera;
        }
    }
}