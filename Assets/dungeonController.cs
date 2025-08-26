using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Dungeon Settings")]
    public GameObject roomPrefab;       
    public int width = 5;               
    public int height = 5;              
    public int maxRooms = 12;

    private int roomsPlaced = 0;
    private Dictionary<Vector2Int, GameObject> dungeonRooms = new Dictionary<Vector2Int, GameObject>();
    private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();
    private Vector2Int[] directions = new Vector2Int[]
    {
        Vector2Int.up,   
        Vector2Int.down,  
        Vector2Int.right, 
        Vector2Int.left   
    };

    void Start()
    {
        Vector2Int startPos = Vector2Int.zero;
        PlaceRoom(startPos);
        roomQueue.Enqueue(startPos);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateNextRoom();
        }
    }

    void GenerateNextRoom()
    {
        if (roomQueue.Count == 0 || roomsPlaced >= maxRooms)
        {
            Debug.Log("No more rooms to generate or max rooms reached!");
            return;
        }

        Vector2Int currentPos = roomQueue.Dequeue();

        foreach (Vector2Int dir in directions)
        {
            Vector2Int newPos = currentPos + dir;

            if (Mathf.Abs(newPos.x) >= width / 2 || Mathf.Abs(newPos.y) >= height / 2)
                continue;

            if (!dungeonRooms.ContainsKey(newPos) && roomsPlaced < maxRooms)
            {
                GameObject newRoom = PlaceRoom(newPos);
                roomQueue.Enqueue(newPos);

                Debug.Log($"Placed room at world position: {newRoom.transform.position}");

                break; 
            }
        }
    }

    Vector3 roomSize = new Vector3(6, 6, 0);
    GameObject PlaceRoom(Vector2Int pos)
    {
        Vector3 worldPos = new Vector3(pos.x * roomSize.x, pos.y * roomSize.y,1f);
        GameObject room = Instantiate(roomPrefab, worldPos, Quaternion.identity, transform);
        dungeonRooms[pos] = room;
        roomsPlaced++;
        return room;
    }
}
