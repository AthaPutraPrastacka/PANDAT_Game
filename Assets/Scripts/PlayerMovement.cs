using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float moveDirection;
    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveDirection = Input.GetAxisRaw("Horizontal");
        animator.SetBool("isWalking", moveDirection != 0);

        if (moveDirection != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveDirection), 1, 1);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection * moveSpeed, rb.linearVelocity.y);
    }
}
