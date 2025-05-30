using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float moveDirection;
    private Animator animator; // Tambahkan Animator

    void Start()
    {
        animator = GetComponent<Animator>(); // Ambil komponen Animator dari Player
    }

    void Update()
    {
        // Ambil input dari tombol A/D atau Left/Right
        moveDirection = Input.GetAxisRaw("Horizontal");

        // Gerakkan karakter hanya maju/mundur (kiri/kanan)
        transform.position += new Vector3(moveDirection * moveSpeed * Time.deltaTime, 0, 0);

        // Update animator parameter
        animator.SetBool("isWalking", moveDirection != 0);
    }
}
