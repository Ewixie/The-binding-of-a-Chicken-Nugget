
using UnityEngine;

namespace Rooms
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private float teleportOffset = 1;
        private bool _isOpen;
        private Door _connectedDoor;

        private Vector2Int _selfDirection;
 

        public void Init(Door connectedDoor, Vector2Int selfDirection)
        {
            Debug.Log("Door init");
            _connectedDoor = connectedDoor;
            _selfDirection = selfDirection;
            Open();
        }

        public Vector2Int GetRelativeToRoomDirection()
        {
            return _selfDirection;
        }

        public float GetTeleportOffset()
        {
            return teleportOffset;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isOpen) return;
            if (other.gameObject.CompareTag("Player"))
            {
                GoThrough(other.transform);
            }
        }

        public void GoThrough(Transform otherTransform)
        {
            Debug.Log("Teleporting...");
            otherTransform.position = _connectedDoor.transform.position;
            otherTransform.position -= ((Vector3) ((Vector2) _connectedDoor.GetRelativeToRoomDirection())) * _connectedDoor.GetTeleportOffset();
        }

        public void Open()
        {
            _isOpen = true;
        }

        public void Close()
        {
            _isOpen = false;
        }
    }
}