using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Managers;
using Player;

namespace UI
{
    public class UpgradeButton : MonoBehaviour
    {
        [SerializeField] private Upgrade upgradeData;

        private Button buyButton;
        private UpgradeManager upgradeManager;

        private void Start()
        {
            buyButton = GetComponent<Button>();
            upgradeManager = FindFirstObjectByType<UpgradeManager>();

            buyButton.onClick.AddListener(BuyUpgrade);
        }
        
        
        private void BuyUpgrade()
        {
            if (upgradeData == null)
            {
                Debug.LogWarning("No upgrade data assigned!");
                return;
            }

            bool success = upgradeManager.TryBuyUpgrade(upgradeData);

            if (success)
            {
                Debug.Log($"Purchased upgrade: {upgradeData.upgradeName}");
                // אם לא רוצים לאפשר קנייה חוזרת:
                // buyButton.interactable = false;
            }
            else
            {
                Debug.Log("Could not purchase upgrade – not enough money?");
            }
        }
        
        // // need to move this to the manager
        // public void BuyUpgrade()
        // {
        //     Debug.Log("Buy button clicked!");
        //     
        //     if (upgradeData == null)
        //     {
        //         Debug.LogWarning("No upgrade data assigned!");
        //         return;
        //     }
        //
        //     if (CurrencyManager.Instance.GetMoney() >= upgradeData.cost)
        //     {
        //         //CurrencyManager.Instance.AddMoney(-upgradeData.cost);
        //         CurrencyManager.Instance.SpendMoney(upgradeData.cost);
        //         ApplyUpgrade();
        //         buyButton.interactable = false;
        //         Debug.Log($"Purchased upgrade: {upgradeData.upgradeName}");
        //     }
        //     else
        //     {
        //         Debug.Log("Not enough money!");
        //     }
        // }
        //
        // private void ApplyUpgrade()
        // {
        //     switch (upgradeData.type)
        //     {
        //         case UpgradeType.ExtraHealth:
        //             Debug.Log("Apply Extra Health Upgrade!");
        //             FindObjectOfType<PlayerController>().IncreaseMaxHealth(1);
        //             // TODO: apply extra health
        //             break;
        //
        //         case UpgradeType.SpeedBoost:
        //             Debug.Log("Apply Speed Boost Upgrade!");
        //             FindObjectOfType<PlayerController>().IncreaseMoveSpeed(1.5f);
        //             // TODO: apply speed boost
        //             break;
        //
        //         case UpgradeType.Dash:
        //             Debug.Log("Unlock Dash Ability!");
        //             // TODO: apply dash ability
        //             break;
        //
        //         case UpgradeType.Hide:
        //             Debug.Log("Unlock Hide Ability!");
        //             // TODO: apply hide ability
        //             break;
        //
        //         default:
        //             Debug.LogWarning("Unknown upgrade type!");
        //             break;
        //     }
        // }
    }
}
