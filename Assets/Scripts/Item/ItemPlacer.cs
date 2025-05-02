using System.Collections.Generic;
using Managers;
using Rooms;
using Scriptable_Objects;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Item
{
    public static class ItemPlacer
    {
        public static void PopulateContainers(List<ItemDefinition> items, List<Room> rooms)
        {
            foreach (var item in items)
            {
                Debug.Log($"placing item {item.itemName} in room {item.roomType}");
                foreach (var room in rooms)
                {
                    Debug.Log(room.RoomType);
                    if (item.roomType == room.RoomType)
                    {
                        room.AddItemToRandomContainer(item);
                    }
                }
            }
        }
    }
}