using System;
using DG.Tweening;
using Managers;
using Player;
using Scriptable_Objects;
using UnityEngine;

namespace Grandma
{
    //TODO: change the system to work with the new rules. (iterate through the list normally) 
    
    public class GrandmaQuestGiver : MonoBehaviour
    {
        [SerializeField] private PlayerInventory playerInventory;
        
        private ItemDefinition _currentItem;
        private bool _isPlayerInRange;
        private int _currentItemIndex;
        
        public event Action OnItemDelivered;
        
        
        private void Start()
        {
            SetNextItemGoal();
        }
        
        private void Update()
        {
            if (!_isPlayerInRange || !Input.GetKeyDown(KeyCode.F)) return; 
            if(playerInventory.CurrentItem == _currentItem) ItemDelivered(playerInventory.CurrentItem);
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _isPlayerInRange = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player")) _isPlayerInRange = false;
        }

        private void ItemDelivered(ItemDefinition delivered)
        {
            if (delivered != _currentItem) return;
            
            playerInventory.DropItem();
            
            OnItemDelivered?.Invoke();
            
            //TODO: REMOVE THIS SHIT ONCE WE HAVE PROPER ANIMATIONS 
            transform
                .DOPunchPosition(Vector3.up * 0.5f, 0.2f, vibrato: 1, elasticity: 0.5f)
                .SetEase(Ease.OutQuad);
            
            if(_currentItemIndex != GameManager.Instance.RecipeItems.Count) SetNextItemGoal();
            else print("All Items Delivered");
        }
        

        private void SetNextItemGoal()
        {
            _currentItem = GameManager.Instance.RecipeItems[_currentItemIndex++];
            print($"current Item is {_currentItem.itemName}");
        }
        
        public bool IsCurrentItem(ItemDefinition item)
        {
            return _currentItem == item;
        }
    }
}