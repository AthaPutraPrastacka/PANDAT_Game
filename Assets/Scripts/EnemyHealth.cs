using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 1;
    private int currentHealth;
    public bool isBoss = false;
    public Level2GameManager level2Manager;
    public Level3GameManager level3Manager;

    // Visual feedback
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Visual feedback - flash red
        if (spriteRenderer)
        {
            StartCoroutine(FlashRed());
        }

        // Update boss health bar
        if (isBoss)
        {
            if (level2Manager)
                level2Manager.UpdateBossHealthBar(currentHealth);
            else if (level3Manager)
                level3Manager.UpdateBossHealthBar(currentHealth);
        }

        // Check if dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    System.Collections.IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    void Die()
    {
        // Store position before destroying
        Vector3 deathPosition = transform.position;
        
        // Notify game manager with position for item drop
        if (level2Manager)
        {
            level2Manager.OnEnemyKilled(isBoss, deathPosition);
        }
        else if (level3Manager)
        {
            level3Manager.OnEnemyKilled(isBoss, deathPosition);
        }
        else if (GameManager.Instance != null && !isBoss)
        {
            // Fallback untuk Level 1
            GameManager.Instance.EnemyKilled();
        }

        // Death animation atau efek bisa ditambahkan di sini
        // Untuk sekarang langsung destroy
        Destroy(gameObject);
    }

    // Called when hit by player attack
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            TakeDamage(1);
        }
    }
}