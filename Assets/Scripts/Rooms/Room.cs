using System;
using System.Collections.Generic;
using System.Linq;
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
        public RoomType RoomType {get; set;}

        private int _fullContainers;
        private bool AllContainersFull => _fullContainers == containers.Count;
        private bool playerInside = false;
        private int currentRound = 1;
        private EnemyPool enemyPool;

        public bool AllContainersFull => _fullContainers == containers.Count;

        private List<IEnemy> enemiesInRoom = new();

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