using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public GameManager gameManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Player terkena musuh!");
            gameManager.GameOver(); // Panggil Game Over
            Destroy(gameObject); // Hapus Player
        }
    }
}
