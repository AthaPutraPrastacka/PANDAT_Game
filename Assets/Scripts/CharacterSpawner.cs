using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public GameObject[] characterPrefabs; // 0 = CharA, 1 = CharB, 2 = CharC
    public Transform spawnPoint;

    void Start()
    {
        int index = GameManager.Instance.selectedCharacter;
        Instantiate(characterPrefabs[index], spawnPoint.position, Quaternion.identity);
    }
}
