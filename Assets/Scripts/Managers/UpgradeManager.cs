using UnityEngine;
using Player;
using Managers;

namespace Managers
{
    public class UpgradeManager : MonoBehaviour
    {
        public PlayerStats playerStats;
        //public int currentCoins = 999;
        
        private void Start()
        {
            if (playerStats == null)
            {
                playerStats = FindFirstObjectByType<PlayerController>().stats;
            }

            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GameScene")
            {
                ReapplyAllUpgrades();
            }
        }


        public bool TryBuyUpgrade(Upgrade upgrade)
        {
            // if (currentCoins >= upgrade.cost)
            // {
            //     currentCoins -= upgrade.cost;
            //     ApplyUpgrade(upgrade);
            //
            //     GameManager.Instance.RegisterUpgrade(upgrade.type);
            //
            //     Debug.Log($"Purchased {upgrade.upgradeName}! Remaining coins: {currentCoins}");
            //     return true;
            // }
            if (CurrencyManager.Instance.GetMoney() >= upgrade.cost)
            {
                CurrencyManager.Instance.SpendMoney(upgrade.cost);
                ApplyUpgrade(upgrade);
                GameManager.Instance.RegisterUpgrade(upgrade.type);
                Debug.Log($"Purchased {upgrade.upgradeName}! Remaining coins: {CurrencyManager.Instance.GetMoney()}");
                return true;
            }
            else
            {
                Debug.Log("Not enough coins to purchase upgrade!");
                return false;
            }
        }

        private void ApplyUpgrade(Upgrade upgrade)
        {
            switch (upgrade.type)
            {
                case UpgradeType.ExtraHealth:
                    // playerStats.maxHealth += 1;
                    FindObjectOfType<PlayerHealth>().IncreaseMaxHealth(1);
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

        public void ReapplyAllUpgrades()
        {
            var upgrades = GameManager.Instance.PurchasedUpgrades;

            foreach (var pair in upgrades)
            {
                for (int i = 0; i < pair.Value; i++)
                {
                    ApplyUpgrade(new Upgrade { type = pair.Key });
                }
            }
            Debug.Log("All saved upgrades reapplied.");
        }
    }
}
