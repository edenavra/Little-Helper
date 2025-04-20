using System;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public PlayerHealth playerHealth;

    private void Start()
    {
        if (playerHealth == null)
        {
            playerHealth = FindObjectOfType<PlayerHealth>();
        }

        healthSlider.maxValue = playerHealth.maxHealth;
        healthSlider.value = playerHealth.getHealth();

        // מאזינים לאירוע
        playerHealth.OnHealthChanged += UpdateHealthUI;
    }

    private void UpdateHealthUI(int current, int max)
    {
        healthSlider.maxValue = max;
        healthSlider.value = current;
    }

    private void OnDestroy()
    {
        // נוודא שמסירים את ההאזנה כשמתאים (כדי למנוע באגים)
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= UpdateHealthUI;
    }
}