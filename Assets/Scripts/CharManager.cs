using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class CharManager : MonoBehaviour
{
    public List<GameObject> characterPrefabs = new List<GameObject>();
    private int selectedChar = 0;
    private GameObject currentPreview;
    public Transform previewParent; // Tempat preview karakter di scene
    public GameObject playerchar;

    public void NextOption()
    {
        selectedChar++;
        if (selectedChar >= characterPrefabs.Count)
        {
            selectedChar = 0;
        }
        ShowCharacter(selectedChar);
    }

    public void BackOption()
    {
        selectedChar--;
        if (selectedChar < 0)
        {
            selectedChar = characterPrefabs.Count - 1;
        }
        ShowCharacter(selectedChar);
    }

    private void ShowCharacter(int index)
    {
        if (currentPreview != null)
        {
            Destroy(currentPreview);
        }
        currentPreview = Instantiate(characterPrefabs[index], previewParent.position, Quaternion.identity, previewParent);
        playerchar = characterPrefabs[index];
    }

    public void PlayGame()
    {
        PlayerPrefs.SetInt("SelectedCharIndex", selectedChar);
        SceneManager.LoadScene("PilihLevel");
    }
}