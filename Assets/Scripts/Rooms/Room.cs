using System;
using System.Collections.Generic;
using System.Linq;
using Scriptable_Objects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Rooms
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private List<Interactable> containers;
        [SerializeField] private EnemySpawner enemySpawner;
        public RoomType RoomType {get; set;}

        private int _fullContainers;
        private bool AllContainersFull => _fullContainers == containers.Count;

        private List<IEnemy> enemiesInRoom = new();

        private void Awake()
        {
            if (enemySpawner == null) enemySpawner = GetComponentInChildren<EnemySpawner>();

            if (enemySpawner != null) enemySpawner.SetRoom(this);
            
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

        public bool AddItemToRandomContainer(ItemDefinition item)
        {
            if (AllContainersFull) return false;
            GetRandomEmptyContainer()?.AddItem(item);
            return true;
        }

        private Interactable GetRandomEmptyContainer()
        {
            var emptyContainers = containers.Where(c => c.IsEmpty).ToList();
            if (emptyContainers.Count == 0) return null;
            int index = Random.Range(0, emptyContainers.Count);
            return emptyContainers[index];
        }
    }
}