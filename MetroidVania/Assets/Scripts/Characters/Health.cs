using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth;
    public int MaxHealth { get => maxHealth; set => maxHealth = value;}
    [SerializeField] GameObject floatingHitPoint;

    [SerializeField] GameObject deathParticles;
    [SerializeField] GameObject tempObjectsParent;

    [SerializeField] GameObject gameOverScreen;

    private bool isDead = false;
    public bool IsDead{get => isDead; set => isDead = value;} 

    private int currentHealth;
    public int CurrentHealth { get => currentHealth; set => currentHealth = value;}

    float gameOverDelay = 1.5f;
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        ShowDamage(damage);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void ShowDamage(int damage)
    {
        if (this.GetComponent<TeamComponent>().teamIndex == TeamIndex.Enemy)
        {
            if (floatingHitPoint != null)
            {
                GameObject damageText = Instantiate(floatingHitPoint, this.transform.position, tempObjectsParent.transform.rotation);
                damageText.transform.GetChild(0).GetComponent<TextMeshPro>().text = damage.ToString();
            }
           
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

    private void Die()
    {
        isDead = true;
        GameObject deadParticles = Instantiate(deathParticles, this.transform.position, this.transform.rotation);
        
        if (this.GetComponent<TeamComponent>().teamIndex == TeamIndex.Enemy)
        {
            this.gameObject.SetActive(false);
        } 
        else if (this.GetComponent<TeamComponent>().teamIndex == TeamIndex.Player)
        {
            StartCoroutine(GameOverCoroutine());
        }
    }

    IEnumerator GameOverCoroutine()
    {
        Time.timeScale = 0.5f;
        gameObject.SetActive(true);
        yield return new WaitForSeconds(gameOverDelay);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
