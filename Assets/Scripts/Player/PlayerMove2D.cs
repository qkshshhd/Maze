using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerMove2D : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float skinWidth = 0.02f;

    private Rigidbody2D rb;
    private Collider2D col;
    private Vector2 moveDirection;
    private bool isMoving;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (isMoving)
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
        if (!isMoving)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (IsWallAhead())
        {
            StopMove();
            return;
        }

        rb.linearVelocity = moveDirection * moveSpeed;
    }

    private void StartMove(Vector2 direction)
    {
        moveDirection = direction;
        isMoving = true;
    }

    private void StopMove()
    {
        isMoving = false;
        moveDirection = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
    }

    private bool IsWallAhead()
    {
        Bounds bounds = col.bounds;

        Vector2 size = bounds.size;
        size.x = Mathf.Max(0.01f, size.x - skinWidth * 2f);
        size.y = Mathf.Max(0.01f, size.y - skinWidth * 2f);

        float distance = moveSpeed * Time.fixedDeltaTime + skinWidth;

        RaycastHit2D hit = Physics2D.BoxCast(
            bounds.center,
            size,
            0f,
            moveDirection,
            distance,
            wallLayer
        );

        return hit.collider != null;
    }
}