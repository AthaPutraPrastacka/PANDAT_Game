using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public float attackInterval = 3f; // Waktu antar serangan
    public GameObject attackEffect; // (opsional) prefab serangan boss
    public Transform attackPoint; // Titik serangan (misalnya arah depan boss)
    public int damage = 1; // Damage ke player
    public float attackRange = 1f; // Radius serangan
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= attackInterval)
        {
            Attack();
            timer = 0f;
        }
    }

    void Attack()
    {
        Debug.Log("BOSS menyerang!");
        GetComponent<Animator>().SetTrigger("Attack");

        // (opsional) efek serangan muncul
        if (attackEffect != null && attackPoint != null)
        {
            Instantiate(attackEffect, attackPoint.position, Quaternion.identity);
        }

        // Cek apakah player kena
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        foreach (Collider2D col in hitPlayers)
        {
            if (col.CompareTag("Player"))
            {
                PlayerHealth health = col.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}