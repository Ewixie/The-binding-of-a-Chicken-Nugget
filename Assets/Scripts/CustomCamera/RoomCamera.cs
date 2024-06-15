using Cinemachine;
using Rooms;
using UnityEngine;

namespace CustomCamera
{
    public class RoomCamera : MonoBehaviour
    {
        [SerializeField] protected CinemachineVirtualCamera virtualCamera;
        [SerializeField] private Room connectedRoom;

        protected virtual void Start()
        {
            foreach (var door in connectedRoom.Doors)
            {
                door.EnteredRoom += (() =>
                {
                    CameraController.Instance.SetCamera(virtualCamera);
                });
            }
        }
    }
}
