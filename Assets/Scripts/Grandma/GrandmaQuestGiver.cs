using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Managers;
using Player;
using Rooms;
using Scriptable_Objects;
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
        
        [SerializeField] private int    hintBlinkCount   = 3;
        [SerializeField] private float  hintDuration   = 2f;
        
        [SerializeField] private SpriteRenderer[] itemUI;
        private SpriteRenderer _activeSprite;
        
        private List<ItemDefinition> _remainingItems;
        [SerializeField] private ItemDefinition book;
        [SerializeField] private ItemDefinition pot;
        [SerializeField] private SpriteRenderer freezerSprite;
        [SerializeField] private SpriteRenderer gardenSprite;
        [SerializeField] private SpriteRenderer basementSprite;
        [SerializeField] private Interactable firstTutorialContainer;
        private ItemDefinition _currentItem;
        private bool _isPlayerInRange;
        private int _currentItemIndex;
        
        public event Action OnItemDelivered;


        private void Start()
        {
            firstTutorialContainer.AddItem(book);
        }

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
            UpdateSpeechBubble();
            print($"current Item is {_currentItem.itemName}");
        }

        private void UpdateSpeechBubble()
        {
            if (_activeSprite != null)
            {
                _activeSprite.enabled = false;
            }
            foreach (var sprite in itemUI)
            {
                if (sprite.name != _currentItem.itemName) continue;
                sprite.enabled = true;
                _activeSprite = sprite;
                ShowLocation();
            }
        }

        private void ShowLocation()
        {
            //disable the item sprite, then show the location sprite, then shaw the item sprite again, repeat 3 times
            if (_currentItemIndex > 3) return;
            switch (_currentItem.roomType)
            {
                case RoomType.Freezer:
                    StartCoroutine(ShowLocationRoutine(freezerSprite));
                    break;
                case RoomType.Garden:
                    StartCoroutine(ShowLocationRoutine(gardenSprite));
                    break;
                case RoomType.Pantry:
                    StartCoroutine(ShowLocationRoutine(basementSprite));
                    break;
                default:
                    return;
            }
        }
        
        private IEnumerator ShowLocationRoutine(SpriteRenderer locSprite)
        {
            for (int i = 0; i < hintBlinkCount; i++)
            {
                locSprite.enabled   = false;
                _activeSprite.enabled = true;
                yield return new WaitForSeconds(hintDuration);
                
                _activeSprite.enabled = false;
                locSprite.enabled   = true;
                yield return new WaitForSeconds(hintDuration);
            }
            locSprite.enabled   = false;
            _activeSprite.enabled = true;
        }

        public bool IsCurrentItem(ItemDefinition item)
        {
            return _currentItem == item;
        }
    }
}