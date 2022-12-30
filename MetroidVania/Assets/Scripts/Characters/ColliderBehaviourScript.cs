using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderBehaviourScript : MonoBehaviour
{
    Collider2D myCollider;
    Health health;

    void Start()
    {
        myCollider = GetComponent<Collider2D>();
        health = GetComponent<Health>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            health.TakeDamage(5);                   
        }
    }
}
