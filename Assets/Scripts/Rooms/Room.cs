using UnityEngine;

namespace Rooms
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private Door topDoor;
        [SerializeField] private Door bottomDoor;
        [SerializeField] private Door leftDoor;
        [SerializeField] private Door rightDoor;

        public Vector2Int RoomIndex { get; set; }

        public void Init()
        {
            topDoor.gameObject.SetActive(false);
            bottomDoor.gameObject.SetActive(false);
            leftDoor.gameObject.SetActive(false);
            rightDoor.gameObject.SetActive(false);
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
