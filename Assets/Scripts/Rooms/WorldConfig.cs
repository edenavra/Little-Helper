using System.Collections.Generic;
using UnityEngine;

namespace Rooms
{
    [CreateAssetMenu(menuName = "World/World Config", fileName = "WorldConfig")]
    public class WorldConfig : ScriptableObject
    {
        [Header("Room Prefabs")]
        public GameObject kitchenPrefab;
        public List<GameObject> pantryLayouts;
        public List<GameObject> freezerLayouts;
        public List<GameObject> gardenLayouts;
        
        [Header("Generation Settings")]
        public float roomSpacing = 20f;
    }
}