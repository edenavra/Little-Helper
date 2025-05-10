using UnityEngine;
using Utils;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] internal int maxHealth = 3;
        private int _currentHealth;
        //public event Action<int, int> OnHealthChanged;

        [SerializeField] private PlayerStats stats;
    
        private PlayerAnimatorController _animatorController;
        private bool _isInvincible = false;

        private void Awake()
        {
            _animatorController = GetComponent<PlayerAnimatorController>();
        }

        private void Start()
        {
            if (stats != null)
            {
                maxHealth = stats.maxHealth;
            }

            _currentHealth = maxHealth;
            //OnHealthChanged?.Invoke(currentHealth, maxHealth);
            GameEvents.PlayerHealthChanged?.Invoke(_currentHealth, maxHealth);
        }
        private void OnEnable()
        {
            GameEvents.RestartLevel += HandleRestart;
        }

        private void OnDisable()
        {
            GameEvents.RestartLevel -= HandleRestart;
        }

        private void HandleRestart()
        {
            _currentHealth = maxHealth;
            //OnHealthChanged?.Invoke(currentHealth, maxHealth);
            GameEvents.PlayerHealthChanged?.Invoke(_currentHealth, maxHealth);
            _isInvincible = false;
        }
        public void TakeDamage(int amount)
        {
            if (_isInvincible) return;
            _currentHealth -= amount;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
            Debug.Log($"Player took {amount} damage. Current health: {_currentHealth}");
            //OnHealthChanged?.Invoke(currentHealth, maxHealth);
            GameEvents.PlayerHealthChanged?.Invoke(_currentHealth, maxHealth);
            if (_currentHealth <= 0)
            {
                Die();
            }
        }
    
        public int GetHealth()
        {
            return _currentHealth;
        }

        private void Die()
        {
            Debug.Log("Player died!");
            //SoundManager.Instance?.PlayGameOver();
            GameEvents.SetUpDeath?.Invoke();
        }

        public void Heal(int amount)
        {
            _currentHealth = Mathf.Min(_currentHealth + amount, maxHealth);
            //OnHealthChanged?.Invoke(currentHealth, maxHealth);
            GameEvents.PlayerHealthChanged?.Invoke(_currentHealth, maxHealth);
            // גם פה אפשר לעדכן UI
        }
    
        public void IncreaseMaxHealth(int amount)
        {
            maxHealth += amount;
            _currentHealth += amount;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
            //OnHealthChanged?.Invoke(currentHealth, maxHealth);
            GameEvents.PlayerHealthChanged?.Invoke(_currentHealth, maxHealth);
            Debug.Log($"[Health] Extra health purchased! MaxHealth: {maxHealth}, CurrentHealth: {_currentHealth}");
        }
    
        public void SetInvincible(bool value)
        {
            _isInvincible = value;
        }
    }
}