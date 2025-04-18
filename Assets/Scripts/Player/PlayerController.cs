using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    private string currentDirection = "";

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        HandleDirection();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movement.normalized * moveSpeed;
    }

    private void HandleDirection()
    {
        if (movement.x > 0 && currentDirection != "Right")
        {
            animator.SetTrigger("GoRight");
            currentDirection = "Right";
        }
        else if (movement.x < 0 && currentDirection != "Left")
        {
            animator.SetTrigger("GoLeft");
            currentDirection = "Left";
        }
        else if (movement.y > 0 && currentDirection != "Up")
        {
            animator.SetTrigger("GoUp");
            currentDirection = "Up";
        }
        else if (movement.y < 0 && currentDirection != "Down")
        {
            animator.SetTrigger("GoDown");
            currentDirection = "Down";
        }
        else if (movement == Vector2.zero)
        {
            currentDirection = "";
        }
    }
}
