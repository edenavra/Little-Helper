using System;
using Scriptable_Objects;
using UnityEngine;
using DG.Tweening; 

namespace Rooms
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private ItemDefinition _storedItem = null;

        [SerializeField] private Transform itemSpawnPoint; 
        
        private float _revealTime = 0.5f;
        
        private bool _isPlayerInRange = false;
        private bool _isOpen = false;
        
        private void Update()
        {
            if (_isPlayerInRange && !_isOpen && Input.GetKeyDown(KeyCode.F))
            {
                Open();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            print("player entered");
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
            if(_storedItem == null) return;
            RevealItem();
        }

        private void RevealItem()
        {
            print("reveal");
            GameObject item = Instantiate(_storedItem.prefab, itemSpawnPoint.position, Quaternion.identity);
            item.transform.localScale = Vector3.zero;
            item.transform.DOScale(Vector3.one, _revealTime).onComplete = OnItemRevealed(item);
        }

        private TweenCallback OnItemRevealed(GameObject item)
        {
            throw new NotImplementedException();
        }
    }
}