using UnityEngine;
using UnityEngine.SceneManagement;

public class Pindah : MonoBehaviour
{
    public void GoToSceneLvl()
    {
        SceneManager.LoadScene("PilihLevel");
    }

    public void GoToSceneChr()
    {
        SceneManager.LoadScene("PilihChara");
    }

    public void GoToLvl1()
    {
        SceneManager.LoadScene("Lvl1");
    }

    public void GoToSetting()
    {
        SceneManager.LoadScene("Setting");
    }

    public void GoToHome()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
