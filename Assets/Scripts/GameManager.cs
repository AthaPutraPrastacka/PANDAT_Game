using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI; // Drag & Drop GameOver UI ke sini di Inspector

    void Start()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }
    }


    public void GameOver()
    {
        Debug.Log("Game Over!");
        gameOverUI.SetActive(true); // Tampilkan Game Over UI
        Time.timeScale = 0f; // Pause game
    }

    public void RestartGame()
    {
        Debug.Log("Restart Game!");
        Time.timeScale = 1f; // Resume game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static GameManager Instance;
    public int selectedCharacter = 0;

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    public void SelectCharacter(int index)
    {
        selectedCharacter = index;
        Debug.Log("Karakter terpilih: " + index);
    }
}
