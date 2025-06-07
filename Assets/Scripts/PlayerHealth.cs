using UnityEngine;
using UnityEngine.SceneManagement; // untuk reload scene jika nyawa habis

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;
    public int currentLives;
    public GameObject[] lifeIcons; // isi dengan UI hati di Inspector (opsional)

    private void Start()
    {
        currentLives = maxLives;
        UpdateLifeUI();
    }

    public void TakeDamage(int amount)
    {
        currentLives -= amount;
        if (currentLives <= 0)
        {
            Die();
        }
        else
        {
            UpdateLifeUI();
        }
    }

    void Die()
    {
        Debug.Log("Player mati");

        // Check for Level3GameManager first (for Level 3)
        Level3GameManager level3Manager = FindObjectOfType<Level3GameManager>();
        if (level3Manager != null)
        {
            level3Manager.GameOver();
        }
        // Then check for Level2GameManager (for Level 2)
        else if (FindObjectOfType<Level2GameManager>() != null)
        {
            FindObjectOfType<Level2GameManager>().GameOver();
        }
        // Then check for regular GameManager (for Level 1)
        else if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
        else
        {
            Debug.LogWarning("No GameManager found!");
        }

        gameObject.SetActive(false);
    }

    void UpdateLifeUI()
    {
        if (lifeIcons.Length == 0) return;
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].SetActive(i < currentLives);
        }
    }
    
    public void Heal(int amount)
    {
        currentLives = Mathf.Min(currentLives + amount, maxLives);
        UpdateLifeUI();
        Debug.Log($"Player healed! Lives: {currentLives}/{maxLives}");
    }
}