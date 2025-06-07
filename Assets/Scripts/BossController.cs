using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("Boss Settings")]
    public float moveSpeed = 1.5f; // Slower movement for more dramatic effect
    public float attackRange = 3f;
    public float aoeRange = 5f;
    public float retreatDistance = 7f; // Distance to retreat from player

    [Header("Attack Settings")]
    public float normalAttackCooldown = 2.5f;
    public float aoeAttackCooldown = 8f;
    public float summonMinionCooldown = 15f;
    public int normalAttackDamage = 20;
    public int aoeAttackDamage = 30;

    [Header("Visual Effects")]
    public GameObject aoeWarningPrefab; // Prefab untuk visual warning AOE
    public GameObject aoeEffectPrefab; // Prefab untuk visual effect AOE
    public float aoeWarningDuration = 1.5f;

    private Transform player;
    private float nextNormalAttack = 0f;
    private float nextAoeAttack = 0f;
    private float nextSummonMinion = 0f;
    private bool isRetreating = false;

    public Level2GameManager level2Manager;
    public Level3GameManager level3Manager;

    // Visual effects
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj) player = playerObj.transform;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Start AI behavior
        StartCoroutine(BossAI());
    }

    IEnumerator BossAI()
    {
        yield return new WaitForSeconds(1f); // Initial delay

        while (player != null && gameObject != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Check if should retreat
            if (distanceToPlayer < retreatDistance && !isRetreating)
            {
                isRetreating = true;
                StartCoroutine(RetreatFromPlayer());
            }
            else if (distanceToPlayer >= retreatDistance)
            {
                isRetreating = false;
            }

            // Move towards player if too far and not retreating
            if (distanceToPlayer > attackRange && !isRetreating)
            {
                MoveTowardsPlayer();
            }

            // Check for attacks
            if (Time.time >= nextAoeAttack && distanceToPlayer <= aoeRange)
            {
                StartCoroutine(PerformAOEAttack());
                nextAoeAttack = Time.time + aoeAttackCooldown;
            }
            else if (Time.time >= nextNormalAttack && distanceToPlayer <= attackRange)
            {
                PerformNormalAttack();
                nextNormalAttack = Time.time + normalAttackCooldown;
            }

            // Summon minions
            if (Time.time >= nextSummonMinion)
            {
                SummonMinions();
                nextSummonMinion = Time.time + summonMinionCooldown;
            }

            yield return new WaitForSeconds(0.1f); // Update rate
        }
    }

    IEnumerator RetreatFromPlayer()
    {
        if (player == null) yield break;

        float retreatTime = 2f;
        float elapsedTime = 0f;
        Vector2 retreatDirection = (transform.position - player.position).normalized;

        while (elapsedTime < retreatTime && isRetreating)
        {
            transform.position += (Vector3)retreatDirection * moveSpeed * 1.5f * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    void MoveTowardsPlayer()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

        // Flip sprite based on direction
        if (spriteRenderer)
        {
            spriteRenderer.flipX = direction.x < 0;
        }

        // Update animation if available
        if (animator)
        {
            animator.SetBool("IsMoving", true);
        }
    }

    void PerformNormalAttack()
    {
        if (animator)
        {
            animator.SetTrigger("Attack");
        }

        // Damage player if in range
        if (player && Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth)
            {
                playerHealth.TakeDamage(normalAttackDamage);
            }
        }
    }

    IEnumerator PerformAOEAttack()
    {
        if (animator)
        {
            animator.SetTrigger("AOEAttack");
        }

        // Show warning
        if (aoeWarningPrefab)
        {
            GameObject warning = Instantiate(aoeWarningPrefab, transform.position, Quaternion.identity);
            Destroy(warning, aoeWarningDuration);
        }

        // Visual warning (flash yellow)
        if (spriteRenderer)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.yellow;
            yield return new WaitForSeconds(aoeWarningDuration);
            spriteRenderer.color = originalColor;
        }

        // Create AOE damage zone
        if (aoeEffectPrefab)
        {
            Instantiate(aoeEffectPrefab, transform.position, Quaternion.identity);
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, aoeRange);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
                if (playerHealth)
                {
                    playerHealth.TakeDamage(aoeAttackDamage);
                }
            }
        }
    }

    void SummonMinions()
    {
        if (animator)
        {
            animator.SetTrigger("Summon");
        }

        GameObject samuraiPrefab = null;
        int minionsToSummon = 2;
        int minionHealth = 1;

        // Check which level manager is active
        if (level2Manager != null)
        {
            samuraiPrefab = level2Manager.samuraiPrefab;
            minionsToSummon = level2Manager.bossMinionCount;
            minionHealth = 3; // Level 2 minions have 3 health
        }
        else if (level3Manager != null)
        {
            samuraiPrefab = level3Manager.samuraiPrefab;
            minionsToSummon = level3Manager.bossMinionCount;
            minionHealth = 6; // Level 3 minions have 6 health
        }

        if (samuraiPrefab == null) return;

        // Summon minions around boss in a circle
        float angleStep = 360f / minionsToSummon;

        for (int i = 0; i < minionsToSummon; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 3f;
            Vector3 spawnPos = transform.position + (Vector3)offset;

            GameObject minion = Instantiate(samuraiPrefab, spawnPos, Quaternion.identity);
            minion.name = "Summoned_Minion_" + i;

            // Setup minion
            EnemyHealth health = minion.GetComponent<EnemyHealth>();
            if (health == null) health = minion.AddComponent<EnemyHealth>();
            health.maxHealth = minionHealth;
            health.isBoss = false;
            
            // Assign appropriate manager
            if (level2Manager != null)
                health.level2Manager = level2Manager;
            else if (level3Manager != null)
                health.level3Manager = level3Manager;
        }
    }

    // Visual indicator for attack range (for debugging)
    void OnDrawGizmosSelected()
    {
        // Normal attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // AOE attack range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aoeRange);

        // Retreat distance
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, retreatDistance);
    }
}