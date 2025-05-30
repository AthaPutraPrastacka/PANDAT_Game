using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // 0=A, 1=B, 2=C
    public Transform spawnPoint;
    public float spawnInterval = 2f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        int selectedCharacter = GameManager.Instance.selectedCharacter;
        GameObject enemyToSpawn = enemyPrefabs[selectedCharacter];
        Instantiate(enemyToSpawn, spawnPoint.position, Quaternion.identity);
    }
}
