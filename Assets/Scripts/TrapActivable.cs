using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapActivable : MonoBehaviour
{
    [SerializeField] private float propulsionForce = 30;
    [SerializeField] private float fallingGrav = 0.7f;
    [SerializeField] private float fanTime = 2f;
    private float actualTime; //Time that the fan is on CD
    private bool isFanOn = true;

    private Animator animator;
    private Rigidbody2D rb;
    [SerializeField] GameObject wind;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        switch (gameObject.tag)
        {
            case "Trampoline":
                {
                    animator.SetFloat("TrapType", 0f);
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
        }
    }

    private void FixedUpdate()
    {
        if (gameObject.CompareTag("Fan"))
        {
            Fan();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Trampoline"))
            {
                Propulse(collision);
                animator.SetTrigger("TrapTrigger");
            }
            else if (gameObject.CompareTag("Falling"))
            {
                Falling();
                animator.SetTrigger("TrapTrigger");
            }
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Trampoline"))
            {
                animator.SetTrigger("TrapOff");
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
    }

    private void Fan()
    {
        actualTime -= Time.deltaTime;
        if(actualTime < 0f && isFanOn)
        {
            wind.SetActive(false);
            isFanOn = !isFanOn;
            actualTime = fanTime;
        }
        else if(actualTime < 0f && !isFanOn)
        {
            wind.SetActive(true);
            isFanOn = !isFanOn;
            actualTime = fanTime;
        }
    }
}
