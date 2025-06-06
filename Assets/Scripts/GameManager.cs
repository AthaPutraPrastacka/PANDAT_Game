using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject gameOverUI;
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;
    public int requiredKillsForBoss = 10;
    private int enemyKillCount = 0;
    private bool bossSpawned = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }
        if (victoryUI != null)
        {
            victoryUI.SetActive(false); // sembunyikan di awal
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Debug.Log("Restart Game!");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EnemyKilled()
    {
        enemyKillCount++;
        Debug.Log("Keroco mati: " + enemyKillCount);

        if (!bossSpawned && enemyKillCount >= requiredKillsForBoss)
        {
            SpawnBoss();
        }
    }

    private void SpawnBoss()
    {
        if (bossPrefab != null && bossSpawnPoint != null)
        {
            Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
            bossSpawned = true;
            Debug.Log("Boss muncul!");
        }
    }
    public bool IsBossSpawned()
    {
        return bossSpawned;
    }

    public GameObject victoryUI;

    public void Victory()
    {
        Debug.Log("PLAYER MENANG 🎉");
        Time.timeScale = 0f; // hentikan waktu
        if (victoryUI != null)
            victoryUI.SetActive(true);
    }
}