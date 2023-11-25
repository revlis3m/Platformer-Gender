using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private AudioSource jumpSoundEffect;
    

    private float movX = 0;

    private enum DoubleJumped { init, jumped, done }
    private DoubleJumped doubleJumped = DoubleJumped.init;
    private enum MovementState { idle, running, jumping, falling, doubleJump }

    private Rigidbody2D playerRigibody;
    private Animator playerAnimator;
    private SpriteRenderer playerSpriteRenderer;
    private BoxCollider2D playerBoxCollider2D;
    // Start is called before the first frame update
    private void Start()
    {
        playerRigibody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        playerBoxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        MovementDetection();
        UpdateAnimation();
    }

    private void MovementDetection()
    {
        movX = Input.GetAxisRaw("Horizontal");
        playerRigibody.velocity = new Vector2(movX * moveSpeed, playerRigibody.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            doubleJumped = DoubleJumped.init;
            playerRigibody.velocity = new Vector2(playerRigibody.velocity.x, jumpForce);
            jumpSoundEffect.Play();
        }

        if (Input.GetButtonDown("Jump") && !IsGrounded() && doubleJumped == DoubleJumped.init)
        {
            doubleJumped = DoubleJumped.jumped;
            playerRigibody.velocity = new Vector2(playerRigibody.velocity.x, jumpForce);
            jumpSoundEffect.Play();
        }
    }

    private void Flip()
    {

    }

    private void UpdateAnimation()
    {
        MovementState state;

        if (movX > 0f)
        {
            state = MovementState.running;
            playerSpriteRenderer.flipX = false;
        }
        else if (movX < 0f)
        {
            state = MovementState.running;
            playerSpriteRenderer.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if(doubleJumped == DoubleJumped.jumped)
        {
            state = MovementState.doubleJump;
            doubleJumped = DoubleJumped.done;
        }

        else if(playerRigibody.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }

        else if(playerRigibody.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        playerAnimator.SetInteger("State", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(playerBoxCollider2D.bounds.center, playerBoxCollider2D.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
