using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Level2GameManager : MonoBehaviour
{
    [Header("Enemy Wave Settings")]
    public GameObject samuraiPrefab;  // Prefab samurai untuk bawahan
    public Transform[] spawnPoints;   // Spawn points untuk musuh
    public int enemiesPerWave = 5;    // Jumlah musuh per wave
    public int totalWaves = 3;        // Total wave di Phase 1
    public float timeBetweenWaves = 5f;
    public float spawnDelay = 0.5f;
    
    [Header("Boss Settings")]
    public GameObject bossPrefab;     // Prefab boss (samurai scaled 2x)
    public Transform bossSpawnPoint;
    public int bossHealth = 10;       // Boss butuh 5x hit
    public int bossMinionCount = 2;   // Jumlah minion yang dipanggil boss
    
    [Header("Item Drop Settings")]
    public GameObject healthDropPrefab;
    public GameObject ammoDropPrefab;
    [Range(0, 100)]
    public int dropChance = 0;       // Persentase drop item (0% = tidak ada drop)
    
    [Header("UI Elements")]
    public Text waveText;
    public Text enemyCountText;
    public Slider bossHealthBar;
    public GameObject bossHealthBarContainer;
    public WaveAnnouncement waveAnnouncement; // Optional wave announcement effect
    
    // Game state
    private int currentWave = 0;
    private int enemiesAlive = 0;
    private int totalEnemiesKilled = 0;
    private bool isWaveActive = false;
    private bool isBossPhase = false;
    private bool gameWon = false;
    
    // References
    private GameObject currentBoss;
    
    void Start()
    {
        // Hide boss health bar at start
        if (bossHealthBarContainer)
            bossHealthBarContainer.SetActive(false);
            
        // Start the first wave
        StartCoroutine(StartWaveSystem());
    }
    
    void Update()
    {
        // Update UI
        UpdateUI();
    }
    
    IEnumerator StartWaveSystem()
    {
        yield return new WaitForSeconds(2f); // Initial delay
        
        while (currentWave < totalWaves && !isBossPhase)
        {
            currentWave++;
            isWaveActive = true;
            
            // Show wave announcement
            string waveMessage = $"Wave {currentWave}/{totalWaves}";
            if (waveAnnouncement != null)
            {
                waveAnnouncement.ShowAnnouncement(waveMessage);
            }
            else if (waveText)
            {
                waveText.text = waveMessage;
            }
                
            yield return new WaitForSeconds(2f);
            
            // Spawn enemies for this wave
            StartCoroutine(SpawnWave());
            
            // Wait until all enemies are killed
            yield return new WaitUntil(() => enemiesAlive == 0);
            
            isWaveActive = false;
            
            // Wave complete
            if (currentWave < totalWaves)
            {
                if (waveText)
                    waveText.text = "Wave Selesai!";
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }
        
        // All waves complete - start boss phase
        if (!isBossPhase)
        {
            StartCoroutine(StartBossPhase());
        }
    }
    
    IEnumerator SpawnWave()
    {
        int enemiesToSpawn = enemiesPerWave + (currentWave - 1) * 2; // Increase enemies each wave
        
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnDelay);
        }
    }
    
    void SpawnEnemy()
    {
        if (samuraiPrefab == null || spawnPoints.Length == 0) return;
        
        // Choose random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        
        // Spawn enemy
        GameObject enemy = Instantiate(samuraiPrefab, spawnPoint.position, Quaternion.identity);
        enemy.name = $"Samurai_Bawahan_{enemiesAlive}";
        
        // Setup enemy components
        EnemyHealth health = enemy.GetComponent<EnemyHealth>();
        if (health == null)
            health = enemy.AddComponent<EnemyHealth>();
            
        health.maxHealth = 3; // Bawahan butuh 3 hit
        health.isBoss = false;
        health.level2Manager = this;
        
        // Setup enemy movement
        EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
        if (movement == null)
            movement = enemy.AddComponent<EnemyMovement>();
            
        enemiesAlive++;
    }
    
    IEnumerator StartBossPhase()
    {
        isBossPhase = true;
        
        // Boss announcement
        string bossMessage = "BOSS MUNCUL!";
        if (waveAnnouncement != null)
        {
            waveAnnouncement.ShowAnnouncement(bossMessage, 60);
        }
        else if (waveText)
        {
            waveText.text = bossMessage;
            waveText.fontSize = 60;
        }
        
        yield return new WaitForSeconds(3f);
        
        // Spawn boss
        SpawnBoss();
    }
    
    void SpawnBoss()
    {
        if (bossPrefab == null || bossSpawnPoint == null) return;
        
        currentBoss = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
        currentBoss.name = "Boss_Samurai";
        
        // Scale boss 2x
        currentBoss.transform.localScale = Vector3.one * 2f;
        
        // Setup boss components
        EnemyHealth health = currentBoss.GetComponent<EnemyHealth>();
        if (health == null)
            health = currentBoss.AddComponent<EnemyHealth>();
            
        health.maxHealth = bossHealth;
        health.isBoss = true;
        health.level2Manager = this;
        
        // Setup boss controller
        BossController bossController = currentBoss.GetComponent<BossController>();
        if (bossController == null)
            bossController = currentBoss.AddComponent<BossController>();
            
        bossController.level2Manager = this;
        
        // Show boss health bar
        if (bossHealthBarContainer)
        {
            bossHealthBarContainer.SetActive(true);
            bossHealthBar.maxValue = bossHealth;
            bossHealthBar.value = bossHealth;
        }
        
        enemiesAlive = 1; // Boss counts as 1 enemy
    }
    
    public void OnEnemyKilled(bool wasBoss, Vector3 enemyPosition)
    {
        enemiesAlive--;
        totalEnemiesKilled++;
        
        // Item drop dinonaktifkan
        /*
        if (!wasBoss)
        {
            // Drop item chance
            TryDropItem(enemyPosition);
        }
        */
        
        if (wasBoss)
        {
            // Boss defeated - Victory!
            StartCoroutine(Victory());
        }
    }
    
    void TryDropItem(Vector3 position)
    {
        int roll = Random.Range(0, 100);
        
        if (roll < dropChance)
        {
            // Randomly choose between health or ammo
            GameObject dropPrefab = Random.Range(0, 2) == 0 ? healthDropPrefab : ammoDropPrefab;
            
            if (dropPrefab != null)
            {
                Instantiate(dropPrefab, position, Quaternion.identity);
            }
        }
    }
    
    public void UpdateBossHealthBar(int currentHealth)
    {
        if (bossHealthBar)
            bossHealthBar.value = currentHealth;
    }
    
    void UpdateUI()
    {
        if (enemyCountText)
        {
            if (!isBossPhase)
                enemyCountText.text = $"Musuh: {enemiesAlive}";
            else
                enemyCountText.text = "BOSS FIGHT!";
        }
    }
    
    IEnumerator Victory()
    {
        gameWon = true;
        
        string victoryMessage = "KEMENANGAN!";
        if (waveAnnouncement != null)
        {
            waveAnnouncement.ShowAnnouncement(victoryMessage, 80);
        }
        else if (waveText)
        {
            waveText.text = victoryMessage;
            waveText.fontSize = 80;
        }
        
        yield return new WaitForSeconds(3f);
        
        // Load next level or main menu
        SceneManager.LoadScene("MainMenu"); // Or next level
    }
    
    public void GameOver()
    {
        // Called when player dies
        Time.timeScale = 0f;
        // Show game over UI
    }
} 