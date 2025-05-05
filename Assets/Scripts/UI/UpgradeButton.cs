using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Managers;
using Player;
using Utils;

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
            Debug.Log(">>> BuyUpgrade called!");

            if (!buyButton.interactable) return;
            if (upgradeData == null)
            {
                Debug.LogWarning("No upgrade data assigned!");
                return;
            }

            bool success = upgradeManager.TryBuyUpgrade(upgradeData);

            if (success)
            {
                Debug.Log($"Purchased upgrade: {upgradeData.upgradeName}");
                //GameEvents.RestartLevel?.Invoke();
                // buyButton.interactable = false;
            }
            else
            {
                Debug.Log("Could not purchase upgrade – not enough money?");
            }
        }
        
    }
}
