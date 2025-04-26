using UnityEngine;
using System.Collections.Generic;
using Player;

namespace Managers
{
    public class UpgradeManager : MonoBehaviour
    {
        public List<Upgrade> availableUpgrades;
        private PlayerStats playerStats;

        private void Start()
        {
            playerStats = FindObjectOfType<PlayerController>().stats;
        }

        public void ApplyUpgrade(Upgrade upgrade)
        {
            switch (upgrade.type)
            {
                case UpgradeType.ExtraHealth:
                    playerStats.maxHealth += 1;
                    break;
                case UpgradeType.SpeedBoost:
                    playerStats.moveSpeed += 2f;
                    break;
                case UpgradeType.Dash:
                    playerStats.dashForce += 5f;
                    break;
                case UpgradeType.Hide:
                    playerStats.hideDuration += 1f;
                    break;
            }

            Debug.Log($"Applied upgrade: {upgrade.upgradeName}");
        }
    }
}