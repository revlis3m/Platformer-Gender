using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBehaviour : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D cl;
    [SerializeField] private AudioClip collectedClip;
    [SerializeField] private AudioClip arrowClip;

    public static event Action OnCollected;
    public static int total;

    void Awake()
    {
        if (gameObject.CompareTag("Orange"))
        {
            total++;
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        cl = GetComponent<BoxCollider2D>();
        switch (gameObject.tag)
        {
            case "Orange":
                {
                    animator.SetFloat("type", 0f);
                }
                break;
            case "Arrow":
                {
                    animator.SetFloat("type", 1f);
                }
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if(gameObject.CompareTag("Orange")) OnCollected?.Invoke();
            PlayerController.orangeCollected++;
            cl.enabled = false;
            if (gameObject.CompareTag("Orange"))
            {
                player.PlaySound(collectedClip);
            }
            else if (gameObject.CompareTag("Arrow"))
            {
                player.PlaySound(arrowClip);
            }
            OnDestroy();
        }
    }

    private void OnDestroy()
    {
        animator.SetTrigger("Collected");
        Invoke("Die", 0.3f);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
