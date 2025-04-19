using System.Collections.Generic;
using Rooms;
using Scriptable_Objects;
using UnityEngine;

namespace Managers
{
    public class RoundManager : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager; 
        [SerializeField] private List<Room> rooms; 

        private int _currentRound = 0;
        private ItemDefinition _currentItem;
        private List<ItemDefinition> _remainingItems;

        
        public ItemDefinition CurrentItem => _currentItem;

        
        private void Start()
        {
            // Clone the run’s placed items so we can whittle them down
            _remainingItems = new List<ItemDefinition>(gameManager.PlacedItems);
            StartNextRound();
        }
        
        public void OnItemDelivered(ItemDefinition delivered)
        {
            if (delivered != _currentItem) return;

            _remainingItems.Remove(delivered);

            StartNextRound();
        }
        
        private void StartNextRound()
        {
            _currentRound++;
        
            //notify all rooms about next round (to increase difficultly)
            foreach (var room in rooms) room.OnRoundStarted(_currentRound);
            SetRandomItemGoal();
        }
        
        public bool IsCurrentObjective(ItemDefinition item)
            => item != null && item == _currentItem;

        private void SetRandomItemGoal()
        {
            _currentItem = _remainingItems[Random.Range(0, _remainingItems.Count)];
        }
    }
}
