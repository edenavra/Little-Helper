namespace Enemies
{
    using System.Collections.Generic;
    using UnityEngine;

    public class EnemyPoolManager : MonoBehaviour
    {
        public static EnemyPoolManager Instance { get; private set; }

        private Dictionary<EnemyType, EnemyPool> pools = new Dictionary<EnemyType, EnemyPool>();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        
            EnemyPool[] allPools = GetComponentsInChildren<EnemyPool>(true);
            foreach (var pool in allPools)
            {
                if (!pools.ContainsKey(pool.Type))
                {
                    pools.Add(pool.Type, pool);
                }
            }
        }

        public EnemyPool GetPool(EnemyType type)
        {
            if (type == EnemyType.None)
                return null;
            if (pools.TryGetValue(type, out var pool))
            {
                return pool;
            }
            Debug.LogError($"No pool found for enemy type {type}");
            return null;
        }
    }

}