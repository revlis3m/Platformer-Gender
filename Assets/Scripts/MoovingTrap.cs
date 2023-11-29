using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoovingTrap : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private float moovingPower = 300f;
    [SerializeField] private float groundCheckDistance = 10f;
    [SerializeField] private LayerMask wall;
    private bool rayUpCollision, rayDownCollision, rayLeftCollision, rayRightCollision;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckCollision();
        if (rayDownCollision && rayLeftCollision)
        {
            Move(Vector2.up);
        }
        else if (rayLeftCollision && rayUpCollision)
        {
            Move(Vector2.right);
        }
        else if (rayRightCollision && rayUpCollision)
        {
            Move(Vector2.down);
        }
        else if (rayDownCollision)
        {
            Move(Vector2.left);
        }
    }

    private void Move(Vector2 direction)
    {
        rb.velocity = Vector2.zero;
        direction = new Vector2(direction.x * moovingPower, direction.y * moovingPower);
        rb.AddForce(direction);
    }

    private void CheckCollision()
    {
        rayUpCollision = Physics2D.Raycast(transform.position, Vector2.up, groundCheckDistance, wall);
        rayDownCollision = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, wall);
        rayLeftCollision = Physics2D.Raycast(transform.position, Vector2.left, groundCheckDistance, wall);
        rayRightCollision = Physics2D.Raycast(transform.position, Vector2.right, groundCheckDistance, wall);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x - groundCheckDistance, transform.position.y));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + groundCheckDistance, transform.position.y));
        
    }

}
