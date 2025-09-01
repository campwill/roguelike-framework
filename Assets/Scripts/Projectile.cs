using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void Start()
    {
        //Destroy(gameObject, 2f); // auto-destroy after 2 seconds
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Don't destroy if it's the player
        if (collision.GetComponent<PlayerController>() != null)
            return;

        // Destroy projectile on any other collider (walls, enemies, etc.)
        Destroy(gameObject);
    }

}
