using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeDestruction : MonoBehaviour
{
    [SerializeField] private Animator orangeAnimator;

    private void Start()
    {
        orangeAnimator = GetComponent<Animator>();
    }

    private void DestroyOrange()
    {
        Destroy(gameObject);
    }

    public void PlayDeath()
    {
        orangeAnimator.SetTrigger("Collected");
    }
}
