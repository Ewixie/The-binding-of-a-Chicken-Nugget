using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

namespace Rooms
{
    public class RoomManager : MonoBehaviour
    {
        [SerializeField] private int seed;
        [SerializeField] private float randomMagicNumber = 0.5f;
        [SerializeField] private Room roomPrefab;
        [SerializeField] private int maxRooms = 10;
        [SerializeField] private int minRooms = 6;

        private readonly int _roomWidth = 20;
        private readonly int _roomHeight = 12;

        private readonly int _gridSizeX = 10;
        private readonly int _gridSizeY = 10;

        private readonly List<Room> _rooms = new List<Room>();

        private Queue<Vector2Int> _roomQueue = new Queue<Vector2Int>();

        private int[,] _roomGrid;

        private bool _generationComplete;

        [Button]
        private void GenerateRandomSeed()
        {
            seed = System.DateTime.Now.Millisecond * Random.Range(-99999, 99999);
            Random.InitState(seed);
        }

        private void Start()
        {
            Random.InitState(seed);

            _roomGrid = new int[_gridSizeX, _gridSizeY];
            _roomQueue = new Queue<Vector2Int>();

            Vector2Int initialRoomIndex = new Vector2Int(_gridSizeX / 2, _gridSizeY / 2);
            StartRoomGenerationFromRoom(initialRoomIndex);
            StartCoroutine(GenerateRooms());
        }

        private IEnumerator GenerateRooms()
        {
            while (true)
            {
                if (_roomQueue.Count > 0 && _rooms.Count < maxRooms && !_generationComplete)
                {
                    Vector2Int roomIndex = _roomQueue.Dequeue();
                    int gridX = roomIndex.x;
                    int gridY = roomIndex.y;

                    TryGenerateRoom(new Vector2Int(gridX - 1, gridY));
                    TryGenerateRoom(new Vector2Int(gridX + 1, gridY));
                    TryGenerateRoom(new Vector2Int(gridX, gridY + 1));
                    TryGenerateRoom(new Vector2Int(gridX, gridY - 1));
                    yield return null;
                }
                else if (!_generationComplete)
                {
                    Debug.Log($"Generation Complete, {_rooms.Count} rooms created");
                    _generationComplete = true;
                    break;
                }
            }
        }

        private void StartRoomGenerationFromRoom(Vector2Int roomIndex)
        {
            int x = roomIndex.x;
            int y = roomIndex.y;
            _roomGrid[x, y] = 1;
            _roomQueue.Enqueue(roomIndex);
            var initialRoom = Instantiate(roomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
            initialRoom.RoomIndex = roomIndex;
            initialRoom.Init();
            _rooms.Add(initialRoom); 
        }

        private bool TryGenerateRoom(Vector2Int roomIndex)
        {
            int x = roomIndex.x;
            int y = roomIndex.y;

            if (x >= _gridSizeX || x < 0) return false;
            if (y >= _gridSizeY || y < 0) return false;

            if (_rooms.Count >= maxRooms)
                return false;
            float random = Random.value;
            if (Random.value < randomMagicNumber) // && roomIndex != Vector2Int.zero
                return false;

            if (CountAdjacentRooms(roomIndex) > 1)
                return false;


            _roomQueue.Enqueue(roomIndex);
            _roomGrid[x, y] = 1;

            var newRoom = Instantiate(roomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
            newRoom.RoomIndex = roomIndex;
            newRoom.name = $"Room-{_rooms.Count}";
            _rooms.Add(newRoom);

            OpenDoors(newRoom, x, y);

            return true;
        }

        void OpenDoors(Room room, int x, int y)
        {
            room.Init();

            Room leftRoomScript = GetRoomAt(new Vector2Int(x - 1, y));
            Room rightRoomScript = GetRoomAt(new Vector2Int(x + 1, y));
            Room topRoomScript = GetRoomAt(new Vector2Int(x, y + 1));
            Room bottomRoomScript = GetRoomAt(new Vector2Int(x, y - 1));

            if (leftRoomScript is not null)
            {
                room.InitDoor(Vector2Int.left, leftRoomScript.GetDoor(Vector2Int.right));
                leftRoomScript.InitDoor(Vector2Int.right, room.GetDoor(Vector2Int.left));
            }

            if (rightRoomScript is not null)
            {
                room.InitDoor(Vector2Int.right, rightRoomScript.GetDoor(Vector2Int.left));
                rightRoomScript.InitDoor(Vector2Int.left, room.GetDoor(Vector2Int.right));
            }

            if (bottomRoomScript is not null)
            {
                room.InitDoor(Vector2Int.down, bottomRoomScript.GetDoor(Vector2Int.up));
                bottomRoomScript.InitDoor(Vector2Int.up, room.GetDoor(Vector2Int.down));
            }

            if (topRoomScript is not null)
            {
                room.InitDoor(Vector2Int.up, topRoomScript.GetDoor(Vector2Int.down));
                topRoomScript.InitDoor(Vector2Int.down, room.GetDoor(Vector2Int.up));
            }
        }

        Room GetRoomAt(Vector2Int index)
        {
            Room roomObject = _rooms.Find(r => r.RoomIndex == index);
            return roomObject ? roomObject : null;
        }

        private int CountAdjacentRooms(Vector2Int roomIndex)
        {
            int x = roomIndex.x;
            int y = roomIndex.y;

            if (x >= _gridSizeX || x < 0) return 0;
            if (y >= _gridSizeY || y < 0) return 0;

            int count = 0;

            if (x > 0 && _roomGrid[x - 1, y] != 0) count++;
            if (x < _gridSizeX - 1 && _roomGrid[x + 1, y] != 0) count++;
            if (y > 0 && _roomGrid[x, y - 1] != 0) count++;
            if (y < _gridSizeY - 1 && _roomGrid[x, y + 1] != 0) count++;

            return count;
        }

        private Vector3 GetPositionFromGridIndex(Vector2Int gridIndex)
        {
            int gridX = gridIndex.x;
            int gridY = gridIndex.y;
            return new Vector3(_roomWidth * (gridX - _gridSizeX / 2),
                _roomHeight * (gridY - _gridSizeY / 2));
        }

        private void OnDrawGizmos()
        {
            Color gizmoColor = new Color(0, 1, 1, 0.05f);
            Gizmos.color = gizmoColor;
            for (int x = 0; x < _gridSizeX; x++)
            {
                for (int y = 0; y < _gridSizeY; y++)
                {
                    Vector3 position = GetPositionFromGridIndex(new Vector2Int(x, y));
                    Gizmos.DrawWireCube(position, new Vector3(_roomWidth, _roomHeight, 1));
                }
            }
        }
    }
}