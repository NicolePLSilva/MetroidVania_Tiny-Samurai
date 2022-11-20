using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth;
    [SerializeField] int currentHealth;
    public int CurrentState { get => currentHealth; set => currentHealth = value;}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TakeDamage(int damage)
    {
        if (currentHealth-damage <= 0)
        {
            currentHealth = 0;
        }
        else
        {
            currentHealth-= damage;
        }
    }

    void Heal()
    {
        currentHealth = maxHealth;
    }

    void Heal(int restore)
    {
        if(currentHealth+restore >= maxHealth)
        {
            Heal();
        }
        else
        {
            currentHealth += restore;
        }
    }

}
