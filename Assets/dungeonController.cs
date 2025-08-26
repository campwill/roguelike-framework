using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Dungeon Settings")]
    public GameObject roomPrefab;       
            
    public int maxRooms = 12;

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


        //need to place the room
        PlaceRoom(start);
        //then add to queue
        frontier.Enqueue(start);
        //then while loop to place the rest of the rooms
        while (frontier.Count > 0 && roomsPlaced < maxRooms )
        {
            Vector2Int current = frontier.Dequeue();
            Debug.Log("FUCK");

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
