using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLive : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    [SerializeField] private GameObject screenMenu;
    [SerializeField] private GameObject endMenu;

    [Header("Checkpoints")]
    [SerializeField] GameObject checkPoint;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        transform.position = new Vector2(checkPoint.transform.position.x, checkPoint.transform.position.y + 5f); ;
        endMenu.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Traps"))
        {
            Die();
        }
    }

    private void Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("dead");
        Invoke("LastCheckpoint", 0.2f);
    }

    private void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LastCheckpoint()
    {
        transform.position = checkPoint.transform.position;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            checkPoint = collision.gameObject;
        }

        else if (collision.gameObject.CompareTag("End"))
        {
            EndLevel();
            
        }
    }

    public void EndLevel()
    {
        screenMenu.SetActive(false);
        endMenu.SetActive(true);
    }
}
