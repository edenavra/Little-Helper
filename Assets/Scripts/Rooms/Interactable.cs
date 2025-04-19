using System;
using Scriptable_Objects;
using UnityEngine;
using DG.Tweening;
using Grandma;
using Player;
using UnityEngine.UIElements;

namespace Rooms
{
    public class Interactable : MonoBehaviour
    {
        
        /// <summary>
        /// TODO FIX BUG WHERE A BOX CANNOT BE OPENED AGAIN ONCE AN ITEM HAS BEEN TAKEN FROM IT 
        /// </summary>
        
        [SerializeField] private Transform itemSpawnPoint;
        private GrandmaQuestGiver _questGiver;
        private PlayerInventory _inventory;

        private float _revealTime = 0.5f;
        private float _collectTime   = 0.2f;
        private float _returnTime    = 0.5f;
        private float _wrongShakeDur = 0.3f;
        private float _wrongShakeStr = .1f;
        private float _jumpPower     = 1f;
        private int   _jumpCount     = 1;
        
        private ItemDefinition _storedItem;
        public bool IsEmpty => _storedItem == null;
        
        private bool _isPlayerInRange;
        private bool _isOpen;
        
        
        private void Awake()
        {
            _questGiver = FindFirstObjectByType<GrandmaQuestGiver>();
            _inventory = FindFirstObjectByType<PlayerInventory>();
        }
        
        private void Update()
        {
            if (_isPlayerInRange && !_isOpen && Input.GetKeyDown(KeyCode.F))
            {
                Open();
            }
        }

        public void AddItem(ItemDefinition item) {_storedItem = item;}
        

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _isPlayerInRange = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player")) _isPlayerInRange = false;
        }

        private void Open()
        {
            print("box opened");
            // Todo: add some kind of feedback that the object is empty
            _isOpen = true;
            if (_storedItem == null)
            {
                print("box is empty");
                return;
            }
            RevealItem();
        }

        private void RevealItem()
        {
            var item = Instantiate(_storedItem.prefab, itemSpawnPoint.position, Quaternion.identity);
            var scale = item.transform.localScale;
            item.transform.localScale = Vector3.zero;
            item.transform
                .DOScale(scale, _revealTime)
                .OnComplete(() => OnItemRevealed(item));
        }
 
        private void OnItemRevealed(GameObject item)
        {
            if (_questGiver.IsCurrentItem(_storedItem))
            {
                // 2) Animate “collect”: jump/arch to the player
                Vector3 target = _inventory.transform.position;
                item.transform
                    .DOJump(target, _jumpPower, _jumpCount, _collectTime)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        // 3) Actually pick it up
                        bool gotIt = _inventory.PickUp(_storedItem);
                        if (!gotIt)
                            Debug.LogWarning("Couldn't pick up — inventory full?");
                        Destroy(item);
                        _storedItem = null;
                    });
            }
            else
            {
                // 1) Wrong item! shake it
                item.transform
                    .DOShakePosition(_wrongShakeDur, _wrongShakeStr)
                    .OnComplete(() =>
                    {
                        // 2) return it to the box
                        item.transform
                            .DOMove(itemSpawnPoint.position, _returnTime)
                            .SetEase(Ease.InOutQuad)
                            .OnComplete(() =>
                            {
                                // allow box to be opened again
                                _isOpen = false;
                                Destroy(item);
                            });
                    });
            }
        }
    }
}