using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    private AudioSource audioSource;
    private string lastSceneName = "";

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        // Jika kembali ke Home dan bukan scene sebelumnya, restart musik
        if (currentScene == "MainMenu" && currentScene != lastSceneName)
        {
            RestartMusic();
        }

        lastSceneName = currentScene;
    }

    void RestartMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.Play();
        }
    }
}
