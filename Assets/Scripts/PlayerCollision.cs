using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public GameManager gameManager;
    private PlayerHealth playerHealth;
    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Player terkena musuh!");

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);

                // Hancurkan musuh
                Destroy(other.gameObject);

                if (playerHealth.currentLives <= 0)
                {
                    gameManager.GameOver();
                    Destroy(gameObject); // hancurkan player
                }
            }
        }
    }
}