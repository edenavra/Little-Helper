using System;
using System.Collections.Generic;
using Rooms;
using Scriptable_Objects;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [Header("Run Setup")] 
        public List<Room> Rooms { get; private set;}
        [SerializeField] private WorldGenerator worldGenerator;
        [SerializeField] private List<ItemDefinition> allItems;

        [SerializeField] internal GameObject playerObject;
        public GameObject PlayerObject => playerObject; 
        
        public IReadOnlyList<ItemDefinition> PlacedItems => allItems;
        
        [SerializeField] private int rounds = 5;

        private void Awake()
        {
            Rooms = worldGenerator.GenerateWorld();
        }

        private void Start()
        {
            PopulateContainers();
        }

        private void PopulateContainers()
        {
            //TODO : THIS IS A PLACE HOLDER FOR MONDAY. CHANGE TO MORE COMPLEX SHIT LATER.
            foreach (var item in allItems)
            {
                bool placed = false;
                while (!placed)
                {
                    var room = Rooms[ChooseRandomRoom()];
                    placed = room.TryAddItemToRandomContainer(item);
                }
            }
        }

        private int ChooseRandomRoom()
        {
            int randomRoomIndex;
            do
            {
                randomRoomIndex = Random.Range(0, Rooms.Count);
            } while(Rooms[randomRoomIndex].AllContainersFull);
            return randomRoomIndex;
        }
    }
}
