using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class SamuraiPrefabSetup : MonoBehaviour
{
    void Reset()
    {
        // Auto setup components when added to GameObject

        // Rigidbody2D setup
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Top-down game, no gravity
        rb.freezeRotation = true;

        // BoxCollider2D setup
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.isTrigger = false; // For collision detection

        // Add required scripts
        if (!GetComponent<EnemyHealth>())
            gameObject.AddComponent<EnemyHealth>();

        if (!GetComponent<EnemyMovement>())
            gameObject.AddComponent<EnemyMovement>();

        // Set tag
        gameObject.tag = "Enemy";

        Debug.Log("Samurai prefab setup complete! Don't forget to:\n" +
                  "1. Assign the Samurai sprite to SpriteRenderer\n" +
                  "2. Adjust collider size to match sprite\n" +
                  "3. Save as prefab in Assets/Prefabs/");
    }
}   