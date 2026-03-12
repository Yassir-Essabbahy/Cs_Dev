using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public GameObject gameOverPanel;
    public TextMeshProUGUI healthText;

    void Start()
    {
        currentHealth = maxHealth;
        gameOverPanel.SetActive(false);
        healthText.text = currentHealth.ToString();
    }


    public void TakeDamage(int amount)
    {
        if (amount <= 0)
        {
            Debug.Log("Invalid Amount");
            return;
        }
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
            
        }
        healthText.text = currentHealth.ToString();
        if (IsDead())
        {
            Die();
        }
    }

    public void Heal(int amount)
    
    {
        if (amount <= 0)
        {
            Debug.Log("Invalid Amount");
            return;
        }

        currentHealth += amount;
        
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
            
        }
        healthText.text = currentHealth.ToString();
    }
    
    public bool IsDead()
    {
        return currentHealth == 0;
    }

    void Die()
    {
        Debug.Log("Player Died");
        gameOverPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }
}
