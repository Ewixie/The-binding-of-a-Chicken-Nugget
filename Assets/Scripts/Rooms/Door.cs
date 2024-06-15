
using System;
using UnityEngine;

namespace Rooms
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private float teleportOffset = 1;
        public bool IsOpen => _isOpen;
        private bool _isOpen;
        private Door _connectedDoor;

        private Vector2Int _selfDirection;
        public event Action EnteredRoom;
        public event Action Opened;
        public event Action Closed;
        
        public void Init(Door connectedDoor, Vector2Int selfDirection)
        {
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
            otherTransform.position = _connectedDoor.transform.position;
            otherTransform.position -= ((Vector3) ((Vector2) _connectedDoor.GetRelativeToRoomDirection())) * _connectedDoor.GetTeleportOffset();
            _connectedDoor.Entered();
        }

        public void Entered()
        {
            EnteredRoom?.Invoke();
        }

        public void Open()
        {
            _isOpen = true;
            Opened?.Invoke();
        }

        public void Close()
        {
            _isOpen = false;
            Closed?.Invoke();
        }
    }
}