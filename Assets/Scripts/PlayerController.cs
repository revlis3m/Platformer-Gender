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
    [SerializeField] private Vector2 wallJumpForce;

    [Header("Collision Infos")]
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask sandGround;
    [SerializeField] private LayerMask iceGround;
    [SerializeField] private LayerMask mudGround;
    [SerializeField] private float groundCheckDistance = 1.1f;
    [SerializeField] private float wallCheckDistance = 0.55f;

    [SerializeField] private AudioClip jumpSound;

    private float movX = 0;
    private bool isGrounded; //Check if i'm on the ground
    private bool canMove = true;
    private bool canDoubleJump = true;
    private bool isFacingRight = true; //If i'm facinf x+ direction
    private bool isWallDetected;
    private bool canWallSlide;
    private bool isWallSliding;
    private bool canWallJump = true; //If can jump off the wall while slide
    private int facingDirection = 1;
    private bool doubleJumped;
    private bool isSand;
    private bool isIce;
    private bool isMud;

    private Rigidbody2D playerRigibody;
    private Animator playerAnimator;
    private AudioSource audioSource;

    private enum AnimationState { idle, running, doubleJump, wallSliding, dead }
    private AnimationState state;

    public static int orangeCollected;
    

    // Start is called before the first frame update
    private void Start()
    {
        playerRigibody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        orangeCollected = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        CheckInput();
        CollisionCheck();
        FlipController();
        AnimationController();
    }

    private void FixedUpdate()
    {
        if (Input.GetAxisRaw("Vertical") < -.1f)
        {
            canWallSlide = false;
        }

        if (isGrounded)
        {
            canDoubleJump = true;
            canMove = true;
        }

        if(isWallDetected && canWallSlide)
        {
            isWallSliding = true;
            playerRigibody.velocity = new Vector3(playerRigibody.velocity.x, playerRigibody.velocity.y * slideForce);
        }
        else if(!isWallDetected)
        {
            isWallSliding = false;
            Move();
        }
    }

    private void CheckInput()
    {
        //Move();
        if (!PauseMenu.isPaused)
        {
            if (Input.GetButtonDown("Jump"))
            {
                JumpButton();
            }

            if(canMove)
            movX = Input.GetAxisRaw("Horizontal");
        }
    }

    private void Move()
    {
        if(canMove)
        playerRigibody.velocity = new Vector2(movX * moveSpeed, playerRigibody.velocity.y);
    }

    private void Flip()
    {
        facingDirection *= -1;
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
        if(isWallSliding && canWallJump)
        {
            WallJump();
        }
        
        else if (isGrounded) Jump();

        else if (canDoubleJump)
        {
            canMove = true;
            canDoubleJump = false;
            doubleJumped = true;
            Jump();
        }

        canWallSlide = false;
    }

    private void Jump()
    {
        playerRigibody.velocity = new Vector2(playerRigibody.velocity.x, jumpForce);
        PlaySound(jumpSound);
    }

    private void WallJump()
    {
        canMove = false;

        Vector2 direction = new Vector2(wallJumpForce.x * -facingDirection, wallJumpForce.y);
        playerRigibody.AddForce(direction, ForceMode2D.Impulse);
    }

    private void AnimationController()
    {
        
        playerAnimator.SetBool("isGrounded", isGrounded);
        playerAnimator.SetFloat("yVelocity", playerRigibody.velocity.y);
        if (movX != 0)
        {
            state = AnimationState.running;
        }
        else
        {
            state = AnimationState.idle;
        }

        if (isWallSliding)
        {
            state = AnimationState.wallSliding;
        }

        if (doubleJumped) 
        { 
            state = AnimationState.doubleJump;
            doubleJumped = false;
        }
        playerAnimator.SetInteger("state", (int)state);
    }

    private void CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, jumpableGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * (isFacingRight ? 1 : -1), wallCheckDistance, jumpableGround);
        isSand = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, sandGround);
        isIce = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, iceGround);
        isMud = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, mudGround);

        if (isSand)
        {
            moveSpeed = 3f;
        }
        else if (isIce)
        {
            moveSpeed = 14f;
        }
        else if (isMud)
        {
            moveSpeed = 0f;
        }
        else
        {
            moveSpeed = 7f;
        }

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

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
