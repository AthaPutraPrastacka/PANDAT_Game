using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType { Health, Ammo }
    public ItemType itemType;
    public int amount = 20; // Amount to restore
    public float floatSpeed = 2f;
    public float floatAmount = 0.5f;
    public float rotateSpeed = 90f;
    
    private Vector3 startPos;
    
    void Start()
    {
        startPos = transform.position;
    }
    
    void Update()
    {
        // Floating animation
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
        
        // Rotation animation
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            switch (itemType)
            {
                case ItemType.Health:
                    PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.Heal(amount);
                        // Visual/audio feedback
                        Debug.Log($"Player healed for {amount} HP!");
                    }
                    break;
                    
                case ItemType.Ammo:
                    PlayerAttack playerAttack = other.GetComponent<PlayerAttack>();
                    if (playerAttack != null)
                    {
                        playerAttack.AddAmmo(amount);
                        Debug.Log($"Player got {amount} ammo!");
                    }
                    break;
            }
            
            // Destroy pickup
            Destroy(gameObject);
        }
    }
} 