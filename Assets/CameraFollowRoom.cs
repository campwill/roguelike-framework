using UnityEngine;

public class CameraFollowRoom : MonoBehaviour
{
    public Transform player;   // Assign your player here

    private Vector2Int lastRoom;

    void LateUpdate()
    {
        Vector2Int currentRoom = new Vector2Int(
            Mathf.FloorToInt((3 + player.position.x) / 6f),
            Mathf.FloorToInt((3+ player.position.y) / 6f)
        );

        if (currentRoom != lastRoom)
        {
            Vector3 roomCenter = new Vector3(currentRoom.x * 6 , currentRoom.y * 6 , -10f);
            transform.position = roomCenter;
            lastRoom = currentRoom;
        }
    }
}
