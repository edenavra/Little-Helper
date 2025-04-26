namespace Enemies
{
    using System.Collections.Generic;
    using UnityEngine;

    public class EnemyPool : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private EnemyType enemyType;
        public EnemyType Type => enemyType;
        private Queue<GameObject> availableEnemies = new Queue<GameObject>();

        public GameObject GetEnemy(Vector2 position)
        {
            GameObject enemy;
            if (availableEnemies.Count > 0)
            {
                enemy = availableEnemies.Dequeue();
                enemy.SetActive(true);
            }
            else
            {
                enemy = Instantiate(enemyPrefab);
            }

            enemy.transform.position = position;
            return enemy;
        }

        public void ReturnEnemy(GameObject enemy)
        {
            enemy.SetActive(false);
            availableEnemies.Enqueue(enemy);
        }
    }

}