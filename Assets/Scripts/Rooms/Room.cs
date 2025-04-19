using System.Collections.Generic;
using Scriptable_Objects;
using UnityEngine;

namespace Rooms
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private List<Interactable> containers;
        [SerializeField] private EnemySpawner enemySpawner;
        private int _fullContainers;
        public bool AllContainersFull => _fullContainers == containers.Count;



        public void OnRoundStarted(int currentRound)
        {
            if (enemySpawner != null)
                enemySpawner.SpawnEnemies(currentRound);
        }

        public bool TryAddItemToRandomContainer(ItemDefinition item)
        {
            if (AllContainersFull)
                return false;

            // try up to containers.Count times to find an empty one
            for (int i = 0; i < containers.Count; i++)
            {
                var index = Random.Range(0, containers.Count);
                if (!containers[index].IsEmpty) continue;
                containers[index].AddItem(item);
                _fullContainers++;
                return true;
            }
            return false;
        }
    }
}