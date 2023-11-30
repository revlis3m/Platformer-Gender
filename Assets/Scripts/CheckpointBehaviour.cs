using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBehaviour : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D boxCollider;
    public static event Action OnCollected;
    public static int total;
    [SerializeField] private AudioClip checkpointSound;

    private void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Awake()
    {
        if (gameObject.CompareTag("Checkpoint"))
        {
            total++;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("playerOnIt", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("playerOnIt", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Checkpoint")) 
            { 
                OnCollected?.Invoke(); 
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.PlaySound(checkpointSound);
            }
            animator.SetTrigger("Checked");
            boxCollider.enabled = false;
        }
    }
}
