using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Dungeon Settings")]
    public GameObject room1DoorPrefab;
    public GameObject room2DoorPrefab;
    public GameObject room3DoorPrefab;
    public GameObject room4DoorPrefab;
    public GameObject tunnelPrefab;
    public int maxRooms = 10;

    private int roomsPlaced = 0;
    private Vector2Int[] direction = new Vector2Int[]
    {
        new Vector2Int(1,0),
        new Vector2Int(-1,0),
        new Vector2Int(0,1),
        new Vector2Int(0,-1),
    };

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
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        Queue<Vector2Int> frontier = new Queue<Vector2Int>();
        Vector2Int start = Vector2Int.zero;

        PlaceRoom(start);
        frontier.Enqueue(start);

        while (frontier.Count > 0 && roomsPlaced < maxRooms)
        {
            Vector2Int current = frontier.Dequeue();
            int branches = Random.Range(1, 4);

            for (int i = 0; i < branches; i++)
            {
                Vector2Int dir = direction[Random.Range(0, directions.Length)];
                Vector2Int newPos = current + dir;

                if (!dungeonRooms.ContainsKey(newPos))
                {
                    PlaceRoom(newPos);
                    frontier.Enqueue(newPos);
                }

                if (roomsPlaced >= maxRooms) break;
            }
        }

        foreach (var pos in new List<Vector2Int>(dungeonRooms.Keys))//surely there is a more efficient way to do this
        {
            UpdateRoom(pos);
        }
    }

    Vector3 roomSize = new Vector3(6, 6, 0);
GameObject PlaceRoom(Vector2Int pos)
{
Vector3 worldPos = new Vector3(
    Mathf.Round(pos.x * roomSize.x),
    Mathf.Round(pos.y * roomSize.y),
    1f
);    
    GameObject room = Instantiate(room1DoorPrefab, worldPos, Quaternion.identity, transform);
    dungeonRooms[pos] = room;
    roomsPlaced++;
    return room;
}
   void UpdateRoom(Vector2Int pos)
{
    if (!dungeonRooms.ContainsKey(pos)) return;

    List<Vector2Int> neighbors = new List<Vector2Int>();
    foreach (var dir in directions)
    {
        if (dungeonRooms.ContainsKey(pos + dir))
            neighbors.Add(dir);
    }

    int doorCount = neighbors.Count;
    GameObject oldRoom = dungeonRooms[pos];
    Vector3 roomPosition = oldRoom.transform.position;
    Transform parent = oldRoom.transform.parent;
    Destroy(oldRoom); //should probably not do it this way, but who cares

    GameObject prefab = null;
    float rotationZ = 0f;

    //do the rotation stuff here
    switch (doorCount)
        {
            case 1:
                prefab = room1DoorPrefab;
                if (neighbors.Contains(Vector2Int.up)) rotationZ = 0f;
                else if (neighbors.Contains(Vector2Int.right)) rotationZ = -90f;
                else if (neighbors.Contains(Vector2Int.down)) rotationZ = 180f;
                else if (neighbors.Contains(Vector2Int.left)) rotationZ = 90f;
                break;

            case 2:
                if ((neighbors.Contains(Vector2Int.up) && neighbors.Contains(Vector2Int.down)))
                {
                    prefab = tunnelPrefab;
                    rotationZ = 0f;
                }
                else if ((neighbors.Contains(Vector2Int.left) && neighbors.Contains(Vector2Int.right)))
                {
                    prefab = tunnelPrefab;
                    rotationZ = 90f;
                }
                else
                {
                    prefab = room2DoorPrefab;
                    if (neighbors.Contains(Vector2Int.up) && neighbors.Contains(Vector2Int.right)) rotationZ = -90f; //im confused wtf is rotation direction
                    else if (neighbors.Contains(Vector2Int.right) && neighbors.Contains(Vector2Int.down)) rotationZ = 180f;
                    else if (neighbors.Contains(Vector2Int.down) && neighbors.Contains(Vector2Int.left)) rotationZ = 90f;
                    else if (neighbors.Contains(Vector2Int.left) && neighbors.Contains(Vector2Int.up)) rotationZ = 0f;
                }
                break;

            case 3:
                prefab = room3DoorPrefab;
                if (!neighbors.Contains(Vector2Int.up)) rotationZ = 180f;
                else if (!neighbors.Contains(Vector2Int.right)) rotationZ = 90f;
                else if (!neighbors.Contains(Vector2Int.down)) rotationZ = 0f;
                else if (!neighbors.Contains(Vector2Int.left)) rotationZ = -90f;
                break;

            case 4:
                prefab = room4DoorPrefab;
                rotationZ = 0f;
                break;
        }

    GameObject newRoom = Instantiate(prefab, roomPosition, Quaternion.Euler(0f, 0f, rotationZ), parent);
    dungeonRooms[pos] = newRoom;
}
}
