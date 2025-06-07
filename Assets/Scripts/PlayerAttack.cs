using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 1;
    public LayerMask enemyLayers;
    public float attackRate = 0.5f;
    
    [Header("Ammo Settings")]
    public bool useAmmo = false; // Toggle untuk menggunakan sistem ammo
    public int maxAmmo = 50;
    public int currentAmmo;

    private float nextAttackTime = 0f;
    
    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void Attack()
    {
        // Cek ammo jika sistem ammo aktif
        if (useAmmo && currentAmmo <= 0)
        {
            Debug.Log("Ammo habis!");
            return;
        }
        
        Debug.Log("Menyerang! ⚔️");
        GetComponent<Animator>().SetTrigger("Attack");
        
        // Kurangi ammo jika menggunakan sistem ammo
        if (useAmmo)
        {
            currentAmmo--;
            Debug.Log($"Ammo: {currentAmmo}/{maxAmmo}");
        }

        // Cek musuh dalam area serangan
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(attackRange, attackRange), 0f, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Musuh terkena serangan! " + enemy.name);

            // Use damage system instead of destroying
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }
            else
            {
                // Fallback for enemies without health system
                Destroy(enemy.gameObject);
            }
        }
    }

    public void ApplyDamage()
    {
        Debug.Log("Serangan mengenai musuh! ⚔️");

        // Cek musuh dalam area serangan saat animasi menyerang
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(attackRange, attackRange), 0f, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Musuh terkena serangan! " + enemy.name);

            // Use damage system instead of destroying
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }
            else
            {
                // Fallback for enemies without health system
                Destroy(enemy.gameObject);
            }
        }
    }


    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, new Vector3(attackRange, attackRange, 0));
    }
    
    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
        Debug.Log($"Got ammo! Current: {currentAmmo}/{maxAmmo}");
    }
}