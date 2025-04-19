using System;
using System.Collections.Generic;
using DG.Tweening;
using Managers;
using Player;
using Scriptable_Objects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Grandma
{
    public class GrandmaQuestGiver : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private PlayerInventory playerInventory;
        
        private List<ItemDefinition> _remainingItems;
        private ItemDefinition _currentItem;
        private bool _isPlayerInRange;
        
        public event Action OnItemDelivered;
        


        private void Start()
        {
            _remainingItems = new List<ItemDefinition>(gameManager.PlacedItems);
            SetRandomItemGoal();
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

            _remainingItems.Remove(delivered);
            
            playerInventory.DropItem();
            
            OnItemDelivered?.Invoke();
            
            //TODO: REMOVE THIS SHIT ONCE WE HAVE PROPER ANIMATIONS 
            transform
                .DOPunchPosition(Vector3.up * 0.5f, 0.2f, vibrato: 1, elasticity: 0.5f)
                .SetEase(Ease.OutQuad);
            
            if(_remainingItems.Count > 0) SetRandomItemGoal();
            else print("All Items Delivered");
        }
        
        private void SetRandomItemGoal()
        {
            _currentItem = _remainingItems[Random.Range(0, _remainingItems.Count)];
            print("current item is: " + _currentItem.name);
        }

        public bool IsCurrentItem(ItemDefinition item)
        {
            return _currentItem == item;
        }
    }
}