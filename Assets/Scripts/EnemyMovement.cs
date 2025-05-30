using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 3f;

    void Update()
    {
        // Gerakkan musuh ke kiri
        transform.position += Vector3.left * speed * Time.deltaTime;
    }
}
