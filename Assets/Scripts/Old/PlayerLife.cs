using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Rigidbody2D playerRigibody;
    private Animator playerAnimator;

    [SerializeField] private AudioSource deathSoundEffect;
    
    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigibody = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Traps"))
        {
            deathSoundEffect.Play();
            Die();
        }
    }

    private void Die()
    {
        playerRigibody.bodyType = RigidbodyType2D.Static;
        playerAnimator.SetTrigger("Death");
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
