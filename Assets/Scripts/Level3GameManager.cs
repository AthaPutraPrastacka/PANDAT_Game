using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Level3GameManager : MonoBehaviour
{
    [Header("Enemy Wave Settings")]
    public GameObject samuraiPrefab;  // Prefab samurai untuk bawahan
    public Transform[] spawnPoints;   // Spawn points untuk musuh
    public int enemiesPerWave = 7;    // Lebih banyak musuh per wave
    public int totalWaves = 4;        // 4 waves untuk Level 3
    public float timeBetweenWaves = 4f;
    public float spawnDelay = 0.4f;   // Spawn lebih cepat
    
    [Header("Boss Settings")]
    public GameObject bossPrefab;     // Prefab boss (samurai scaled 2x)
    public Transform bossSpawnPoint;
    public int bossHealth = 20;       // Boss butuh 20 hit untuk Level 3
    public int bossMinionCount = 3;   // Boss summon lebih banyak minion
    
    [Header("Item Drop Settings")]
    public GameObject healthDropPrefab;
    public GameObject ammoDropPrefab;
    [Range(0, 100)]
    public int dropChance = 0;       // Tetap 0% sesuai permintaan
    
    [Header("UI Elements")]
    public Text waveText;
    public Text enemyCountText;
    public Slider bossHealthBar;
    public GameObject bossHealthBarContainer;
    public WaveAnnouncement waveAnnouncement;
    
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
        // Level 3: Increase enemies more aggressively each wave
        int enemiesToSpawn = enemiesPerWave + (currentWave - 1) * 3;
        
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
        enemy.name = $"Samurai_Bawahan_L3_{enemiesAlive}";
        
        // Setup enemy components
        EnemyHealth health = enemy.GetComponent<EnemyHealth>();
        if (health == null)
            health = enemy.AddComponent<EnemyHealth>();
            
        health.maxHealth = 6; // Level 3: Musuh butuh 6 hit
        health.isBoss = false;
        health.level2Manager = null; // Clear level 2 reference
        health.level3Manager = this; // Set level 3 reference
        
        // Setup enemy movement with increased speed for Level 3
        EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
        if (movement == null)
            movement = enemy.AddComponent<EnemyMovement>();
        
        // Increase enemy stats for Level 3
        if (movement.speed < 3f)
            movement.speed = 3f; // Faster enemies
        movement.attackDamage = 15; // More damage than Level 2
            
        enemiesAlive++;
    }
    
    IEnumerator StartBossPhase()
    {
        isBossPhase = true;
        
        // Boss announcement
        string bossMessage = "BOSS LEVEL 3 MUNCUL!";
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
        currentBoss.name = "Boss_Samurai_L3";
        
        // Scale boss 2x
        currentBoss.transform.localScale = Vector3.one * 2f;
        
        // Setup boss components
        EnemyHealth health = currentBoss.GetComponent<EnemyHealth>();
        if (health == null)
            health = currentBoss.AddComponent<EnemyHealth>();
            
        health.maxHealth = bossHealth; // Level 3: Boss butuh 20 hit
        health.isBoss = true;
        health.level3Manager = this;
        
        // Setup boss controller with enhanced stats for Level 3
        BossController bossController = currentBoss.GetComponent<BossController>();
        if (bossController == null)
            bossController = currentBoss.AddComponent<BossController>();
        
        // Enhance boss for Level 3
        bossController.level3Manager = this;
        bossController.normalAttackDamage = 30; // More damage
        bossController.aoeAttackDamage = 40;
        bossController.normalAttackCooldown = 2f; // Attack more frequently
        bossController.aoeAttackCooldown = 6f;
        bossController.summonMinionCooldown = 12f;
        
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
        
        // Item drop dinonaktifkan sesuai permintaan
        /*
        if (!wasBoss && dropChance > 0)
        {
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
                enemyCountText.text = "BOSS FIGHT LEVEL 3!";
        }
    }
    
    IEnumerator Victory()
    {
        gameWon = true;
        
        string victoryMessage = "LEVEL 3 SELESAI!";
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
        
        // Load next level or credits
        SceneManager.LoadScene("MainMenu"); // Or "Credits" scene
    }
    
    public void GameOver()
    {
        // Called when player dies
        Time.timeScale = 0f;
        // Show game over UI
        if (waveText)
        {
            waveText.text = "GAME OVER";
            waveText.fontSize = 60;
        }
    }
} 