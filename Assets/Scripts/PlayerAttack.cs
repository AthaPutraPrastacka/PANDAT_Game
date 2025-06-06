using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 1;
    public LayerMask enemyLayers;
    public float attackRate = 0.5f;

    private float nextAttackTime = 0f;

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
        Debug.Log("Menyerang! ⚔️");
        GetComponent<Animator>().SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(attackRange, attackRange), 0f, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Musuh terkena serangan! " + enemy.name);

            // Cek apakah musuh punya EnemyHealth
            EnemyHealth health = enemy.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(attackDamage);
            }
            else
            {
                Destroy(enemy.gameObject); // untuk keroco biasa
                GameManager.Instance.EnemyKilled(); // jika perlu hitung kill
            }
        }
    }
    public void ApplyDamage()
    {
        Debug.Log("Serangan mengenai musuh! ⚔️");
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(attackRange, attackRange), 0f, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Musuh terkena serangan! " + enemy.name);

            EnemyHealth health = enemy.GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(attackDamage);
            }
            else
            {
                Destroy(enemy.gameObject);
                GameManager.Instance.EnemyKilled();
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
}
