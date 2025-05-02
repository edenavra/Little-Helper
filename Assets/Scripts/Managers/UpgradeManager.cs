using UnityEngine;
using Player;
using Managers;

namespace Managers
{
    public class UpgradeManager : MonoBehaviour
    {
        private PlayerStats playerStats;

        private void Start()
        {
            // Grab PlayerController to access stats
            var controller = FindFirstObjectByType<PlayerController>();

            if (controller != null)
            {
                playerStats = controller.stats;

                // Set default values for player stats !!
                playerStats.hideDuration = 20f;
                playerStats.hideCooldown = 20f;

                Debug.Log($"[UpgradeManager] Linked to actual PlayerStats. HideDuration: {playerStats.hideDuration}");
            }
            else
            {
                Debug.LogError("[UpgradeManager] PlayerController not found! Cannot link stats.");
                return;
            }

            // Apply saved upgrades
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
                    FindObjectOfType<PlayerHealth>()?.IncreaseMaxHealth(1);
                    break;

                case UpgradeType.SpeedBoost:
                    int level = GameManager.Instance.PurchasedUpgrades.ContainsKey(UpgradeType.SpeedBoost)
                        ? GameManager.Instance.PurchasedUpgrades[UpgradeType.SpeedBoost]
                        : 0;
                    float bonus = Mathf.Max(0.5f, 2f - level * 0.3f);
                    playerStats.moveSpeed += bonus;
                    Debug.Log($"[UpgradeManager] Speed Boost applied, level {level}, bonus {bonus}, new speed {playerStats.moveSpeed}");
                    break;

                case UpgradeType.FreezerTime:
                    playerStats.extraFreezeTime += 5f; // או כל ערך שתרצי
                    Debug.Log($"[UpgradeManager] Freezer time increased! Bonus: {playerStats.extraFreezeTime} seconds");
                    break;


                case UpgradeType.Hide:
                    int hideLevel = GameManager.Instance.PurchasedUpgrades.ContainsKey(UpgradeType.Hide)
                        ? GameManager.Instance.PurchasedUpgrades[UpgradeType.Hide]
                        : 0;

                    float newCooldown = Mathf.Max(5f, 20f - hideLevel * 3f);
                    playerStats.hideCooldown = newCooldown;

                    Debug.Log($"[UpgradeManager] Hide upgraded to level {hideLevel + 1}. New cooldown is {newCooldown} seconds.");
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
