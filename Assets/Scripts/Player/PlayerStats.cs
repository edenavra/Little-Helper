using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class PlayerStats
    {
        [Header("Health Settings")]
        public int maxHealth = 100;

        [Header("Movement Settings")]
        public float moveSpeed = 5f;

        [Header("Dash Settings")]
        public float dashForce = 10f;

        [Header("Hide Settings")]
        public float hideDuration = 20f;
        public float hideCooldown = 20f;

    }
}