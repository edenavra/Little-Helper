using System;
using System.Collections.Generic;
using DG.Tweening;
using Managers;
using Player;
using Rooms;
using Scriptable_Objects;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

namespace Grandma
{
    public class GrandmaQuestGiver : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private PlayerInventory playerInventory;
        
        [SerializeField] private WorldConfig worldConfig;
        
        [SerializeField] private Transform givenItemContainer;
        
        private List<ItemDefinition> _remainingItems;
        private ItemDefinition _currentItem;
        private bool _isPlayerInRange;
        private int _currentItemIndex;
        
        public event Action OnItemDelivered;
        

        private void OnEnable()
        {
            GameEvents.StartQuest += StartQuest;
        }
        private void OnDisable()
        {
            GameEvents.StartQuest -= StartQuest;
        }
        private void StartQuest()
        {
            _currentItemIndex = 0;
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

        private void ItemDelivered(ItemDefinition itemDelivered)
        {
            if (itemDelivered != _currentItem) return;
            
            playerInventory.DropItem();
            
            OnItemDelivered?.Invoke();
            
            //TODO: REMOVE THIS SHIT ONCE WE HAVE PROPER ANIMATIONS 
            //grandma jump
            // transform
            //     .DOPunchPosition(Vector3.up * 0.5f, 0.2f, vibrato: 1, elasticity: 0.5f)
            //     .SetEase(Ease.OutQuad);
            
            //item animation
            var item = Instantiate(itemDelivered.prefab, playerInventory.transform.position, Quaternion.identity, givenItemContainer);
            item.transform.localScale = itemDelivered.scale;
            item.transform.localRotation = itemDelivered.rotation;
            item.transform
                .DOJump(itemDelivered.deliveryPosition, 3, 1, 2)
                .SetEase(Ease.OutQuad);
            
            if(_currentItemIndex != worldConfig.recipeItems.Count) SetNextItemGoal();
            else
            {
                print("All Items Delivered");
                GameEvents.PlayerWon?.Invoke();
            }
        }


        private void SetNextItemGoal()
        {
            _currentItem = worldConfig.recipeItems[_currentItemIndex++];
            print($"current Item is {_currentItem.itemName}");
        }
        
        public bool IsCurrentItem(ItemDefinition item)
        {
            return _currentItem == item;
        }
    }
}