using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLive : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    [SerializeField] private GameObject screenMenu;
    [SerializeField] private GameObject endMenu;

    [Header("Checkpoints")]
    [SerializeField] GameObject checkPoint;

    [SerializeField] Image star1;
    [SerializeField] Image star2;
    [SerializeField] Image star3;

    private float ratio;
    [SerializeField] private AudioClip endClip;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private float levelBottom = -30f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        transform.position = new Vector2(checkPoint.transform.position.x, checkPoint.transform.position.y + 5f); ;
        endMenu.SetActive(false);
        ratio = 0;
        star1.enabled = false;
        star2.enabled = false;
        star3.enabled = false;
    }

    private void Update()
    {
        if (gameObject.transform.position.y == levelBottom) Die();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Traps"))
        {
            PlayerController player = gameObject.GetComponent<PlayerController>();
            player.PlaySound(deathClip);
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
            PlayerController player = gameObject.GetComponent<PlayerController>();
            player.PlaySound(endClip);
            EndLevel();
        }
    }

    public void EndLevel()
    {
        screenMenu.SetActive(false);
        endMenu.SetActive(true);
        ratio = (float)(PlayerController.orangeCollected / 2) / (float)CollectibleBehaviour.total;
        
        if (ratio >= 1f)
        {
            star3.enabled = true;
        }
        if (ratio >= 0.6f)  star2.enabled = true;
        if (ratio >= 0.3f)  star1.enabled = true;
    }
}
