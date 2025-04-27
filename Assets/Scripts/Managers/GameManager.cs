using System.Collections.Generic;
using Item;
using Rooms;
using Scriptable_Objects;
using UnityEngine;
using Utils;

namespace Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [Header("Run Setup")] 
        [SerializeField] private int rounds = 5;
        [SerializeField] private List<ItemDefinition> recipeItems;
        [SerializeField] private List<ItemDefinition> trashItems;
        [SerializeField] private WorldGenerator worldGenerator;
        [SerializeField] internal GameObject playerObject;
        public  List<Room> Rooms { get; private set;}
        
        public List<ItemDefinition> PlacedItems => recipeItems;
        public List<ItemDefinition> TrashItems => trashItems;
        public GameObject PlayerObject => playerObject; 


        private void Awake()
        {
            Rooms = worldGenerator.GenerateWorld();
            ItemPlacer.PopulateContainers(recipeItems);
            ItemPlacer.PopulateContainers(TrashItems);
        }

        private void Start()
        {
        }
    }
}
