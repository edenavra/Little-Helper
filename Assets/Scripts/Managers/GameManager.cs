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
        [Header("Run Setup")] [SerializeField] private List<Room> rooms;
        [SerializeField] private List<ItemDefinition> allItems;

        [SerializeField] internal GameObject playerObject;
        public GameObject PlayerObject => playerObject; 


    public IReadOnlyList<ItemDefinition> PlacedItems => allItems;
        
        [SerializeField] private int rounds = 5;

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
                    var room = rooms[ChooseRandomRoom()];
                    placed = room.TryAddItemToRandomContainer(item);
                }
            }
        }

        private int ChooseRandomRoom()
        {
            int randomRoomIndex;
            do
            {
                randomRoomIndex = Random.Range(0, rooms.Count);
            } while(rooms[randomRoomIndex].AllContainersFull);
            return randomRoomIndex;
        }
    }
}
