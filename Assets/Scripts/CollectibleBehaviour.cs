using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBehaviour : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
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
