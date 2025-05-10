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
            UpdateButtonState();
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
                UpdateButtonState();
                //GameEvents.RestartLevel?.Invoke();
                // buyButton.interactable = false;
            }
            else
            {
                Debug.Log("Could not purchase upgrade – not enough money?");
            }
        }
        
        private void UpdateButtonState()
        {
            if (upgradeManager.HasReachedLimit(upgradeData.type))
            {
                buyButton.interactable = false;

                var colors = buyButton.colors;
                colors.disabledColor = new Color(0.3f, 0.3f, 0.3f);
                buyButton.colors = colors;
            }
        }

        
    }
}
