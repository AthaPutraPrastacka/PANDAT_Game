using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public List<GameObject> characterPrefabs = new List<GameObject>();
    public Transform spawnPoint;

    void Start()
    {
        int selectedChar = PlayerPrefs.GetInt("SelectedCharIndex", 0);

        if (selectedChar >= 0 && selectedChar < characterPrefabs.Count && characterPrefabs[selectedChar] != null)
        {
            Instantiate(characterPrefabs[selectedChar], spawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Selected character index is out of range or prefab is null.");
        }
    }
}