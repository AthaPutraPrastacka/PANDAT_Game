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

        // Cek musuh dalam area serangan
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(attackRange, attackRange), 0f, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Musuh terkena serangan! " + enemy.name);
            Destroy(enemy.gameObject); // Hancurkan musuh
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
            Destroy(enemy.gameObject); // Hancurkan musuh
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
