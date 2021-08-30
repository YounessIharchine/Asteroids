using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameManager gameManager;
    public Asteroid asteroidPrefab;

    public float spawnRate = 3.0f;
    public int spawnAmount = 2;
    public float spawnDistance = 11.0f;
    public float angleVariance = 15.0f;

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), 0.0f, spawnRate);
    }

    private void Spawn()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector2 spawnPoint = Random.insideUnitCircle.normalized * spawnDistance;
            float variance = Random.Range(-angleVariance, angleVariance);
            Quaternion rotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.up, -spawnPoint.normalized) + variance, Vector3.forward);

            Asteroid asteroid = Instantiate(asteroidPrefab, spawnPoint, rotation);

            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);

            Quaternion trajectoryVariance = Quaternion.AngleAxis(variance, Vector3.forward);
            asteroid.SetTrajectory(trajectoryVariance * -spawnPoint.normalized);

            asteroid.gameManager = gameManager;
        }
    }
}
