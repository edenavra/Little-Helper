using UnityEngine;
using Player;
using Managers;

namespace Managers
{
    public class UpgradeManager : MonoBehaviour
    {
        private  PlayerStats playerStats;
        
        private void Start()
        {
            if (playerStats == null)
            {
                var controller = FindFirstObjectByType<PlayerController>();
                playerStats = controller.stats;

                Debug.Log($"[UpgradeManager] Linked to actual PlayerStats. Initial moveSpeed: {playerStats.moveSpeed}");
            }

            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GameScene")
            {
                ReapplyAllUpgrades();
            }
        }



        public bool TryBuyUpgrade(Upgrade upgrade)
        {
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
                    FindObjectOfType<PlayerHealth>().IncreaseMaxHealth(1);
                    break;
                case UpgradeType.SpeedBoost:
                    //playerStats.moveSpeed += 2f;
                    int level = GameManager.Instance.PurchasedUpgrades.ContainsKey(UpgradeType.SpeedBoost)
                        ? GameManager.Instance.PurchasedUpgrades[UpgradeType.SpeedBoost]
                        : 0;

                    float bonus = Mathf.Max(0.5f, 2f - level * 0.3f); // הולך וקטן, אבל לא יורד מ־0.5
                    playerStats.moveSpeed += bonus;

                    Debug.Log($"[UpgradeManager] Speed Boost applied, level {level}, bonus {bonus}, new speed {playerStats.moveSpeed}");
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
