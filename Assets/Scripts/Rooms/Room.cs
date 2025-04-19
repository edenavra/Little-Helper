using System;
using System.Collections.Generic;
using Scriptable_Objects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Rooms
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private List<Interactable> containers;
        [SerializeField] private EnemySpawner enemySpawner;

        private int _fullContainers;
        public bool AllContainersFull => _fullContainers == containers.Count;

        private List<IEnemy> enemiesInRoom = new List<IEnemy>();

        private void Awake()
        {
            if (enemySpawner == null)
                enemySpawner = GetComponentInChildren<EnemySpawner>();

            if (enemySpawner != null)
            {
                enemySpawner.SetRoom(this);
            }

            IEnemy[] existingEnemies = GetComponentsInChildren<IEnemy>(includeInactive: true);
            foreach (var enemy in existingEnemies)
            {
                enemiesInRoom.Add(enemy);
            }
        }

        public void RegisterEnemy(GameObject enemyObj)
        {
            IEnemy enemy = enemyObj.GetComponent<IEnemy>();
            if (enemy != null && !enemiesInRoom.Contains(enemy))
            {
                enemiesInRoom.Add(enemy);
            }
        }

        public void OnRoundStarted(int currentRound)
        {
            if (enemySpawner != null)
                enemySpawner.SpawnEnemies(currentRound);

            foreach (var enemy in enemiesInRoom)
            {
                enemy.OnRoundStarted(currentRound);
            }
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