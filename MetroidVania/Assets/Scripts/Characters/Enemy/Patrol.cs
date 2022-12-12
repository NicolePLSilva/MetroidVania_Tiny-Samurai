using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float distanceRay = 2f;
    [SerializeField] LayerMask terrainMask;
    [SerializeField] LayerMask platformMask;
    [SerializeField] Transform groundDetection;

    bool movingRight = true;
    Vector2 direction = Vector2.right;
    public Vector2 Direction { get => direction; set => direction = value; }

    bool mustPatrol = false;
    public bool MustPatrol { get => mustPatrol; set => mustPatrol = value; }

    Animator animator;
    RaycastHit2D wallInfo;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PatrolProcess()
    {
        if (mustPatrol)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            animator.SetBool("IsRunning", true);
        }

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distanceRay, terrainMask);
        RaycastHit2D platformInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distanceRay, platformMask);
        wallInfo = Physics2D.Raycast(groundDetection.position, direction, distanceRay, terrainMask);

        Debug.DrawRay(groundDetection.position, direction, Color.red);
        if ((groundInfo.collider == false || wallInfo.collider == true) && mustPatrol)
        {
            if (movingRight)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                movingRight = false;
                direction = Vector2.left;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
                direction = Vector2.right;
            }
        }
    }
}
