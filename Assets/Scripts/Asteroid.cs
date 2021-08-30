using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Sprite[] sprites;
    
    private SpriteRenderer spriteRenderer;
    private new Rigidbody2D rigidbody;

    public float minSize = 0.5f;
    public float maxSize = 1.5f;
    public float speed;
    public float maxAngleVariance;
    public float minAngleVariance;

    [HideInInspector]
    public GameManager gameManager;
    public float size;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Spawn(size);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            if (size >= minSize * 3.0f / 2.0f)
            {
                float variance = Random.Range(minAngleVariance, maxAngleVariance);
                SpawnSplit(variance);
                SpawnSplit(-variance);
            }
            gameManager.DestroyAsteroid(this);
        }

        if (collision.tag == "Player")
        {
            gameManager.DestroyAsteroid(this);
        }

        if (collision.tag == "Asteroid Killer")
        {
            Destroy(gameObject);
        }
    }

    private void Spawn(float size)
    {
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        transform.localScale = Vector3.one * size;
        rigidbody.mass = size;
    }

    private void SpawnSplit(float variance)
    {
        Vector2 position = transform.position;
        position += (Vector2)(Quaternion.AngleAxis(variance, Vector3.forward) * (0.5f * (Vector2)transform.up));

        Quaternion rotation = transform.rotation * Quaternion.AngleAxis(variance, Vector3.forward);

        Asteroid asteroid = Instantiate(this, position, rotation);
        asteroid.size = size * 2.0f / 3.0f;

        asteroid.SetTrajectory((asteroid.transform.position - transform.position).normalized);

        asteroid.gameManager = gameManager;
    }

    public void SetTrajectory(Vector2 direction)
    {
        rigidbody.AddForce(speed * direction);
    }
}
