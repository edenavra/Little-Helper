using Managers;
using System;
using UnityEngine;
using Player;
using Utils;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] internal int maxHealth = 3;
    private int currentHealth;
    public event Action<int, int> OnHealthChanged;

    [SerializeField] private PlayerStats stats;

    private void Start()
    {
        if (stats != null)
        {
            maxHealth = stats.maxHealth;
        }

        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"Player took {amount} damage. Current health: {currentHealth}");
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public int getHealth()
    {
        return currentHealth;
    }

    private void Die()
    {
        Debug.Log("Player died!");
        // כאן אפשר לקרוא לפונקציית Game Over או להפעיל אנימציה וכו'
        //SoundManager.Instance?.PlayGameOver();
        GameEvents.PlayerDied?.Invoke();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        // גם פה אפשר לעדכן UI
    }
    
    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        Debug.Log($"[Health] Extra health purchased! MaxHealth: {maxHealth}, CurrentHealth: {currentHealth}");
    }

}