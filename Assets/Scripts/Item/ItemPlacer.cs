using System.Collections.Generic;
using Managers;
using Scriptable_Objects;
using UnityEngine;

namespace Item
{
    public static class ItemPlacer
    {
        public static void PopulateContainers(List<ItemDefinition> items)
        {
            foreach (var item in items)
            {
                foreach (var room in GameManager.Rooms)
                {
                    if (item.roomType == room.RoomType)
                    {
                        room.AddItemToRandomContainer(item);
                    }
                }
            }
        }
    }
}