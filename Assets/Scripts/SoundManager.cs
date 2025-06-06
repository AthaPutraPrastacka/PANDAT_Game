using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    private static SoundManager instance;

    private void Awake()
    {
        // Cegah duplikat saat pindah scene
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Ambil volume dari setting (atau default 1f jika belum ada)
        float savedVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
        AudioListener.volume = savedVolume;

        // Jika slider di-assign, sinkronkan tampilannya & event-nya
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(ChangeVolume);
        }
        else
        {
            Debug.LogWarning("Volume slider belum di-assign.");
        }
    }

    // Dipanggil saat slider digeser
    public void ChangeVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("musicVolume", value);
    }

    // Opsional: agar bisa set slider dari scene lain
    public void SetVolumeSlider(Slider newSlider)
    {
        volumeSlider = newSlider;
        volumeSlider.value = AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }
}
