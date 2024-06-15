
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

namespace Rooms
{
    public class RoomManager : MonoBehaviour
    {
        [SerializeField] private bool generateRandomSeedOnStart = true;
        [SerializeField] private int seed;
        [SerializeField] private float randomMagicNumber = 0.5f;
        [SerializeField] private Room startRoom;
        [SerializeField] private Room endRoom;
        [SerializeField] private List<Room> roomPrefabs;
        [SerializeField] private int maxRooms = 10;
        [SerializeField] private int minRooms = 6;

        private bool _endRoomSpawned;
        
        private readonly int _roomWidth = 20;
        private readonly int _roomHeight = 12;

        private readonly int _gridSizeX = 10;
        private readonly int _gridSizeY = 10;

        private readonly List<Room> _rooms = new List<Room>();

        private Queue<Vector2Int> _roomQueue = new Queue<Vector2Int>();

        private int[,] _roomGrid;

        private Vector2Int _startIndex;

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

#if !UNITY_EDITOR
            generateRandomSeedOnStart = true;
#endif

            if (generateRandomSeedOnStart) GenerateRandomSeed();
            _roomGrid = new int[_gridSizeX, _gridSizeY];
            _roomQueue = new Queue<Vector2Int>();

            Vector2Int initialRoomIndex = new Vector2Int(_gridSizeX / 2, _gridSizeY / 2);
            StartRoomGenerationFromRoom(initialRoomIndex);
            GenerateRooms();
            SpawnFinalRoom(GetPerfectRooms(_rooms));

            foreach (var room in _rooms)
            {
                OpenDoors(room, room.RoomIndex.x, room.RoomIndex.y);
            }
        }

        private void SpawnFinalRoom(List<Room> rooms)
        {
            Room randomRoom = rooms[Random.Range(0, rooms.Count)];
            Vector2Int index = randomRoom.RoomIndex;

            _rooms.Remove(randomRoom);
            Destroy(randomRoom.gameObject);
            
            var finalRoom = Instantiate(endRoom, GetPositionFromGridIndex(index), Quaternion.identity);
            finalRoom.transform.parent = transform;
            finalRoom.RoomIndex = index;
            finalRoom.Init();
            _rooms.Add(finalRoom);
        }

        private List<Room> GetPerfectRooms(List<Room> rooms)
        {
            //найти все комнаты с одним соседом
            List<Room> roomsWithNoNeighbours = new List<Room>(rooms);
            foreach (var room in roomsWithNoNeighbours.ToList())
            {
                if (CountAdjacentRooms(room.RoomIndex) > 1)
                {
                    roomsWithNoNeighbours.Remove(room);
                }
            }
            //найти все комнаты, которые рядом со стартовой комнатой
            List<Room> adjacentToStartRooms = new List<Room>();
            Room leftRoomScript = GetRoomAt(new Vector2Int(_startIndex.x - 1, _startIndex.y));
            Room rightRoomScript = GetRoomAt(new Vector2Int(_startIndex.x + 1, _startIndex.y));
            Room topRoomScript = GetRoomAt(new Vector2Int(_startIndex.x, _startIndex.y + 1));
            Room bottomRoomScript = GetRoomAt(new Vector2Int(_startIndex.x, _startIndex.y - 1));
            if (leftRoomScript is not null) adjacentToStartRooms.Add(leftRoomScript);
            if (rightRoomScript is not null) adjacentToStartRooms.Add(rightRoomScript);
            if (topRoomScript is not null) adjacentToStartRooms.Add(topRoomScript);
            if (bottomRoomScript is not null) adjacentToStartRooms.Add(bottomRoomScript);
            //найти их разницу между ними
            List<Room> perfectForBossRooms = new List<Room>(roomsWithNoNeighbours);
            foreach (var room in adjacentToStartRooms)
            {
                perfectForBossRooms.Remove(room);
            }
            //если в этой разнице что-то есть, то берём случайную комнату оттуда
            if (perfectForBossRooms.Count > 0)
            {
                //replace room with boss room
                return perfectForBossRooms;
            }

            //иначе мы берём из того, что осталось :(
            if (roomsWithNoNeighbours.Count > 0)
            {
                return roomsWithNoNeighbours;
            }

            List<Room> roomsNotAdjacentToStart = new List<Room>(rooms);
            foreach (var room in adjacentToStartRooms)
            {
                roomsNotAdjacentToStart.Remove(room);
            }

            if (roomsNotAdjacentToStart.Count > 0)
            {
                return roomsWithNoNeighbours;
            }

            //any from rooms but not start
            List<Room> iDontGiveAFuckAtThisPoint = new List<Room>(rooms);
            iDontGiveAFuckAtThisPoint.Remove(GetRoomAt(_startIndex));
            return iDontGiveAFuckAtThisPoint;
        }

        private void GenerateRooms()
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
                }
                else if (!_generationComplete)
                {
                    if (_rooms.Count >= minRooms)
                    {
                        Debug.Log($"Generation Complete, {_rooms.Count} rooms created");
                        _generationComplete = true;
                        break;
                    }
                    else
                    {
                        Debug.Log($"Generation incomplete, stopped at {_rooms.Count}. Trying to generate more...");
                        Vector2Int roomIndex = _rooms.Last().RoomIndex;
                        int gridX = roomIndex.x;
                        int gridY = roomIndex.y;

                        TryGenerateRoom(new Vector2Int(gridX - 1, gridY));
                        TryGenerateRoom(new Vector2Int(gridX + 1, gridY));
                        TryGenerateRoom(new Vector2Int(gridX, gridY + 1));
                        TryGenerateRoom(new Vector2Int(gridX, gridY - 1));
                    }
                }
            }
        }

        private void StartRoomGenerationFromRoom(Vector2Int roomIndex)
        {
            _startIndex = roomIndex;
            int x = roomIndex.x;
            int y = roomIndex.y;
            _roomGrid[x, y] = 1;
            _roomQueue.Enqueue(roomIndex);
            var initialRoom = Instantiate(startRoom, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
            initialRoom.RoomIndex = roomIndex;
            initialRoom.transform.parent = transform;
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
            if (random < randomMagicNumber)
                return false;
            if (roomIndex == _startIndex)
                return false;
            if (CountAdjacentRooms(roomIndex) > 1)
                return false;
            if (_roomGrid[x, y] != 0)
                return false;


            _roomQueue.Enqueue(roomIndex);
            _roomGrid[x, y] = 1;

            var randomRoomPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Count)];
            var newRoom = Instantiate(randomRoomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
            newRoom.RoomIndex = roomIndex;
            newRoom.name = $"Room-{_rooms.Count}";
            newRoom.transform.parent = transform;
            _rooms.Add(newRoom);
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