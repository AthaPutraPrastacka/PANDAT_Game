using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public List<GameObject> characterPrefabs = new List<GameObject>();
    public Transform spawnPoint;

    void Start()
    {
        int selectedChar = PlayerPrefs.GetInt("SelectedCharIndex", 0);

        if (characterPrefabs.Count > 0 && characterPrefabs[0] != null)
        {
            Instantiate(characterPrefabs[0], spawnPoint.position, Quaternion.identity);
        }
    }
}