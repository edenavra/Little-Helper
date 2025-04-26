using System;
using System.Collections.Generic;
using Enemies;
using Scriptable_Objects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Rooms
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private List<Interactable> containers;
        [SerializeField] private bool hasEnemies = false;
        [SerializeField] private EnemySpawner enemySpawner;
        [SerializeField] private EnemyType enemyTypeNeededForThisRoom;
        //[SerializeField] private EnemyPool enemyPool;

        private int _fullContainers;
        private bool playerInside = false;
        private int currentRound = 1;
        private EnemyPool enemyPool;

        public bool AllContainersFull => _fullContainers == containers.Count;

        private List<IEnemy> enemiesInRoom = new List<IEnemy>();

        private void Awake()
        {
            if (enemySpawner == null)
            {
                if (hasEnemies)
                {
                    enemySpawner = GetComponentInChildren<EnemySpawner>();
                    if (enemySpawner == null)
                    {
                        Debug.LogError("EnemySpawner is missing in this room with enemies!");
                    }
                }
            }

            if (hasEnemies && enemySpawner != null)
            {
                enemySpawner.SetRoom(this);
            }

            IEnemy[] existingEnemies = GetComponentsInChildren<IEnemy>(includeInactive: true);
            foreach (var enemy in existingEnemies)
            {
                enemiesInRoom.Add(enemy);
            }
        }

        private void Start()
        {
            if (hasEnemies)
            {
                enemyPool = EnemyPoolManager.Instance.GetPool(enemyTypeNeededForThisRoom);
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
            this.currentRound = currentRound;
        }
        private void UpdateEnemyLevel(int round)
        {
            foreach (var enemy in enemiesInRoom)
            {
                enemy.OnRoundStarted(round);
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
        
        public void OnPlayerExited()
        {
            foreach (var enemy in enemiesInRoom)
            {
                if (enemy is MonoBehaviour enemyMono) 
                {
                    enemyPool.ReturnEnemy(enemyMono.gameObject);
                }
            }
            enemiesInRoom.Clear();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerInside = true;
                if (enemySpawner != null && hasEnemies && enemyPool != null)
                {
                    enemySpawner.SpawnEnemies(currentRound, enemyPool);
                }
                
                UpdateEnemyLevel(currentRound);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerInside = false;
                if (hasEnemies && enemyPool != null)
                {
                    UpdateEnemyLevel(1);
                    OnPlayerExited();
                }
            }
        }

    }
}