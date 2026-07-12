using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove2D : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private string wallTag = "Wall";

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private bool canAcceptInput = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!canAcceptInput)
            return;

        if (Input.GetKeyDown(KeyCode.W))
            StartMove(Vector2.up);
        else if (Input.GetKeyDown(KeyCode.S))
            StartMove(Vector2.down);
        else if (Input.GetKeyDown(KeyCode.A))
            StartMove(Vector2.left);
        else if (Input.GetKeyDown(KeyCode.D))
            StartMove(Vector2.right);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = canAcceptInput
            ? Vector2.zero
            : moveDirection * moveSpeed;
    }

    private void StartMove(Vector2 direction)
    {
        moveDirection = direction;
        canAcceptInput = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryStopByWall(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryStopByWall(collision);
    }

    private void TryStopByWall(Collision2D collision)
    {
        if (canAcceptInput)
            return;

        if (!collision.collider.CompareTag(wallTag))
            return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (Vector2.Dot(contact.normal, moveDirection) < -0.5f)
            {
                StopMove();
                return;
            }
        }
    }

    private void StopMove()
    {
        moveDirection = Vector2.zero;
        canAcceptInput = true;
        rb.linearVelocity = Vector2.zero;
    }
}