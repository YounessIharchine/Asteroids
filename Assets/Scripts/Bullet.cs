using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 8.0f;
    public float timeAlive = 1.25f;

    private new Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Project(Vector2 direction)
    {
        rigidbody.velocity = direction * speed;
        Destroy(gameObject, timeAlive);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Asteroid")
            Destroy(gameObject);

        if (collision.tag == "TeleporterX")
        {
            transform.position = new Vector3(-transform.position.x, transform.position.y, 0);
        }
        if (collision.tag == "TeleporterY")
        {
            transform.position = new Vector3(transform.position.x, -transform.position.y, 0);
        }
    }
}
