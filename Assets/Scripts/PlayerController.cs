using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

public class PlayerController : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float slideForce;

    [Header("Collision Infos")]
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float groundCheckDistance = 1.1f;
    [SerializeField] private float wallCheckDistance = 0.55f;
    

    private float movX = 0;
    private bool isGrounded; //Check if i'm on the ground
    private bool canMove = true;
    private bool canDoubleJump = true;
    private bool isFacingRight = true; //If i'm facinf x+ direction
    private bool isWallDetected;
    private bool canWallSlide;
    private bool isWallSliding;


    private Rigidbody2D playerRigibody;

    // Start is called before the first frame update
    private void Start()
    {
        playerRigibody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        CheckInput();
        CollisionCheck();
        FlipController();
    }

    private void FixedUpdate()
    {
        if (isGrounded)
        {
            canDoubleJump = true;
        }

        if(isWallDetected && canWallSlide)
        {
            isWallSliding = true;
            playerRigibody.velocity = new Vector3(playerRigibody.velocity.x, playerRigibody.velocity.y * slideForce);
        }
        else
        {
            isWallSliding = false;
            Move();
        }
    }

    private void CheckInput()
    {
        //Move();
        if (Input.GetButtonDown("Jump"))
        {
            JumpButton();
        }

        if(canMove)
        movX = Input.GetAxisRaw("Horizontal");
    }

    private void Move()
    {
        if(canMove)
        playerRigibody.velocity = new Vector2(movX * moveSpeed, playerRigibody.velocity.y);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    private void FlipController()
    {
        if(isGrounded && isWallSliding)
        {
            if(isFacingRight && movX < -.1f)
            {
                Flip();
            }
            else if(!isFacingRight && movX > .1f)
            {
                Flip();
            }
        }

        if(playerRigibody.velocity.x > .1f && !isFacingRight)
        {
            Flip();
        }
        else if (playerRigibody.velocity.x < -.1f && isFacingRight) 
        {
            Flip();
        }
    }

    private void JumpButton()
    {
        if (isGrounded) Jump();

        else if (canDoubleJump)
        {
            canDoubleJump = false;
            Jump();
        }
    }

    private void Jump()
    {
        playerRigibody.velocity = new Vector2(playerRigibody.velocity.x, jumpForce);
    }

    private void CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, jumpableGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * (isFacingRight ? 1 : -1), wallCheckDistance, jumpableGround);

        if(!isGrounded && playerRigibody.velocity.y < -.1f)
        {
            canWallSlide = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector2 (transform.position.x,transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2 (transform.position.x + wallCheckDistance * (isFacingRight? 1:-1),transform.position.y));
    }
}
