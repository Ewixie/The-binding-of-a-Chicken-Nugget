using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rooms
{
    public class Room : MonoBehaviour
    {
        [Header("Doors")]
        [SerializeField] private Door topDoor;
        [SerializeField] private Door bottomDoor;
        [SerializeField] private Door leftDoor;
        [SerializeField] private Door rightDoor;

        public IEnumerable<Door> Doors => _allDoors;
        private readonly List<Door> _allDoors = new List<Door>();
        
        public Vector2Int RoomIndex { get; set; }

        private void Awake()
        {
            _allDoors.Add(topDoor);
            _allDoors.Add(bottomDoor);
            _allDoors.Add(leftDoor);
            _allDoors.Add(rightDoor);
        }

        public void Init()
        {
            foreach(var door in _allDoors) door.gameObject.SetActive(false);
        }

        public void OpenDoors()
        {
            foreach (var door in _allDoors.Where(door => door.gameObject.activeSelf))
            {
                door.Open();
            }
        }

        public void CloseDoors()
        {
            foreach (var door in _allDoors.Where(door => door.gameObject.activeSelf))
            {
                door.Close();
            }
        }
        
        public void InitDoor(Vector2Int direction, Door connectedDoor)
        {
            var door = GetDoor(direction);
            if (door is null) return;
            door.gameObject.SetActive(true);
            door.Init(connectedDoor,  direction);
        }

        public Door GetDoor(Vector2Int direction)
        {
            if (direction == Vector2Int.up)
            {
                return topDoor;
            }
            if (direction == Vector2Int.down)
            {
                return bottomDoor;
            }
            if (direction == Vector2Int.left)
            {
                return leftDoor;
            }
            if (direction == Vector2Int.right)
            {
                return rightDoor;
            }

            return null;
        }
    }
}
