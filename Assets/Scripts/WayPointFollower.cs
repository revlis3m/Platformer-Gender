using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WayPointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int currentWayPointIndex = 0;
    private bool playerOn = false;

    [SerializeField] private float speed = 2f;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        switch (gameObject.tag)
        {
            case "MoovingPlatform":
                {
                    animator.SetFloat("type", 1f);
                }
                break;
            case "WaitingPlatform":
                {
                    animator.SetFloat("type", 0f);
                }
                break;
        }
    }

    private void Update()
    {
        if (gameObject.CompareTag("MoovingPlatform"))
        {
            if (Vector2.Distance(waypoints[currentWayPointIndex].transform.position, transform.position) < .1f)
            {
                currentWayPointIndex++;
                if(currentWayPointIndex >= waypoints.Length)
                {
                    currentWayPointIndex = 0;
                }
            }
            transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWayPointIndex].transform.position, speed * Time.deltaTime);
            animator.SetBool("mooving", true);
        }
        else if (gameObject.CompareTag("WaitingPlatform"))
        {
            if (playerOn)
            {
                if (Vector2.Distance(waypoints[currentWayPointIndex].transform.position, transform.position) < .1f)
                {
                    currentWayPointIndex++;
                    if (currentWayPointIndex >= waypoints.Length)
                    {
                        currentWayPointIndex = 0;
                    }
                }
                transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWayPointIndex].transform.position, speed * Time.deltaTime);
                animator.SetBool("mooving", true);
            }
            else if (!playerOn)
            {
                transform.position = Vector2.MoveTowards(transform.position, waypoints[0].transform.position, speed * Time.deltaTime);
                if(new Vector2(transform.position.x,transform.position.y) == new Vector2(waypoints[0].transform.position.x, waypoints[0].transform.position.y))
                {
                    animator.SetBool("mooving", false);
                }
            }
        }
        else
        {
            if (Vector2.Distance(waypoints[currentWayPointIndex].transform.position, transform.position) < .1f)
            {
                currentWayPointIndex++;
                if (currentWayPointIndex >= waypoints.Length)
                {
                    currentWayPointIndex = 0;
                }
            }
            transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWayPointIndex].transform.position, speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOn = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOn = false;
        }
    }
}
