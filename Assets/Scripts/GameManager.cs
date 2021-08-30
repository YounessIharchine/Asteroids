using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player player;
    public ParticleSystem explosion;
    public Asteroid asteroidPrefab;
    public TextMeshProUGUI scoreText;
    public GameOver gameOver;
    public Transform Lives;

    public float respawnTime = 1.5f;
    public float invincibilityTime = 3.0f;
    public int timesFlickering = 8;
    public float minScore = 300.0f;
    public float maxScore = 600.0f;

    private float score = 0.0f;
    private int lives = 2;

    public void KillPlayer()
    {
        AudioManager.instance.Play("Player Explode");

        var emitParams = new ParticleSystem.EmitParams();
        emitParams.position = player.transform.position;
        emitParams.applyShapeToPosition = true;
        explosion.Emit(emitParams, 10);

        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
        player.transform.position = Vector3.zero;
        player.transform.rotation = Quaternion.identity;
        player.gameObject.SetActive(false);

        lives--;


        Invoke(nameof(RespawnPlayer), respawnTime);

        if (lives < 0)
        {
            Time.timeScale = 0.0f;
            GameOver.isGameOver = true;
            gameOver.gameOverUI.SetActive(true);
        }
        else
        {
            Lives.GetChild(lives).GetComponent<Image>().enabled = false;
        }
    }

    private void RespawnPlayer()
    {
        player.gameObject.layer = LayerMask.NameToLayer("IgnoreCollision");
        player.gameObject.SetActive(true);
        player.canShoot = true;

        StartCoroutine(InvincibilityFlicker(invincibilityTime, timesFlickering));
        Invoke(nameof(RemovePlayerInvincibility), invincibilityTime);
    }

    private IEnumerator InvincibilityFlicker(float invincibilityTime, int timesFlickering)
    {
        float timePerHalfFlicker = invincibilityTime / timesFlickering / 2;
        for (int i = 0; i < timesFlickering; i++)
        {
            player.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(timePerHalfFlicker);
            player.GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(timePerHalfFlicker);
        }
    }

    private void RemovePlayerInvincibility()
    {
        player.gameObject.layer = LayerMask.NameToLayer("Player");
    }


    public void DestroyAsteroid(Asteroid asteroid)
    {
        AudioManager.instance.Play("Asteroid Explode");

        var emitParams = new ParticleSystem.EmitParams();
        emitParams.position = asteroid.transform.position;
        emitParams.applyShapeToPosition = true;
        explosion.Emit(emitParams, 10);

        Destroy(asteroid.gameObject);

        float bonusScore = (asteroidPrefab.maxSize - asteroid.size) / (asteroidPrefab.maxSize - asteroidPrefab.minSize) * (maxScore - minScore);

        score += minScore + bonusScore;
        int intScore = (int)score;
        scoreText.text = intScore.ToString();
    }
}
