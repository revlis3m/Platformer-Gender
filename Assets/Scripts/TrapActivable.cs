using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapActivable : MonoBehaviour
{
    [SerializeField] private float propulsionForce = 30;
    [SerializeField] private float fallingGrav = 0.7f;
    [SerializeField] private float fanTime = 2f;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float trapTimeActive = 0.28f;
    private float actualTime; //Time that the fan is on CD
    private bool isFanOn = true;
    [SerializeField] private AudioClip fireClip;
    [SerializeField] private AudioClip trampolineClip;
    private AudioSource audioSource;

    private Animator animator;
    private Rigidbody2D rb;
    [SerializeField] GameObject addons; //Wind, Fire
    [SerializeField] ParticleSystem flyParticles;
    [SerializeField] private LayerMask crashableLand;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        switch (gameObject.tag)
        {
            case "Trampoline":
                {
                    animator.SetFloat("TrapType", 0f);
                    audioSource = GetComponent<AudioSource>();
                }
                break;

            case "Falling":
                {
                    animator.SetFloat("TrapType", 1f);
                }
                break;
            case "Fan":
                {
                    animator.SetFloat("TrapType", 2f);
                    actualTime = fanTime;
                }
                break;
            case "Fire":
                {
                    animator.SetFloat("TrapType", 3f);
                    addons.SetActive(false);
                    audioSource = GetComponent<AudioSource>();
                }
                break;
        }
    }

    private void FixedUpdate()
    {
        if (gameObject.CompareTag("Fan"))
        {
            Fan();
        }
        if (CollisionCheck())
        {
            OnDestroy();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Trampoline"))
            {
                TrapActivable trap = gameObject.GetComponent<TrapActivable>();
                //if(trampolineClip != null)
                //trap.PlayClip(trampolineClip);
                Propulse(collision);
                animator.SetTrigger("TrapTrigger");
            }
            else if (gameObject.CompareTag("Falling"))
            {
                Falling();
                animator.SetTrigger("TrapTrigger");
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Fire"))
            {
                TrapActivable trap = gameObject.GetComponent<TrapActivable>();
                trap.PlayClip(fireClip);
                animator.SetTrigger("Transition");
                Invoke("Fire", trapTimeActive);
            }
        }
    }

    private void OnDestroy()
    {
        Destroy(gameObject);
    }

    private void Propulse(Collision2D collision)
    {
        Vector2 direction = new Vector2(0, propulsionForce);
        collision.rigidbody.AddForce(direction, ForceMode2D.Impulse);
    }

    private void Falling()
    {
        rb.gravityScale = fallingGrav;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.bodyType = RigidbodyType2D.Dynamic;
        flyParticles.Stop();
    }

    private bool CollisionCheck()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, crashableLand);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
    }

    private void Fan()
    {
        actualTime -= Time.deltaTime;
        if(actualTime < 0f && isFanOn)
        {
            addons.SetActive(false);
            isFanOn = !isFanOn;
            actualTime = fanTime;
        }
        else if(actualTime < 0f && !isFanOn)
        {
            addons.SetActive(true);
            isFanOn = !isFanOn;
            actualTime = fanTime;
        }
    }

    private void Fire()
    {
        
        addons.SetActive(true);
        Invoke("NoFire", 0.5f);
    }

    private void NoFire()
    {
        animator.SetTrigger("TrapOff");
        addons.SetActive(false);
    }

    private void PlayClip(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
