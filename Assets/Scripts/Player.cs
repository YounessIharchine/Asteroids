using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float thrustSpeed = 6.0f;
    public float turnSpeed = 1.0f;
    public float waitShoot = 0.25f;
    public Bullet bulletPrefab;
    public GameManager gameManager;

    private new Rigidbody2D rigidbody;
    private bool thrusting;
    private float turnDirection;
    [HideInInspector]
    public bool canShoot;


    private void Awake()
    {
        canShoot = true;
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        thrusting = Input.GetKey(KeyCode.W);

        if (Input.GetKey(KeyCode.D))
        {
            turnDirection = -1.0f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            turnDirection = 1.0f;
        }
        else
        {
            turnDirection = 0.0f;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (canShoot)
            {
                Shoot();
                StartCoroutine(WaitForShoot());
            }
        }
    }

    private void FixedUpdate()
    {
        if (thrusting)
        {
            rigidbody.AddForce(transform.up * thrustSpeed);
        }
        if (turnDirection != 0)
        {
            rigidbody.AddTorque(turnDirection * turnSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Asteroid")
            gameManager.KillPlayer();
        if (collision.tag == "TeleporterX")
        {
            transform.position = new Vector3(-transform.position.x, transform.position.y, 0);
        }
        if (collision.tag == "TeleporterY")
        {
            transform.position = new Vector3(transform.position.x, -transform.position.y, 0);
        }
    }

    private void Shoot()
    {
        Bullet bullet = Instantiate(bulletPrefab, transform.position + transform.up / 2.7f, Quaternion.identity);
        bullet.Project(transform.up);
        AudioManager.instance.Play("Shoot");
    }

    private IEnumerator WaitForShoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(waitShoot);
        canShoot = true;
    }
}
