using Scriptable_Objects;
using UnityEngine;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        public ItemDefinition CurrentItem { get; private set; }
        
        public bool PickUp(ItemDefinition item)
        {
            if(CurrentItem != null) return false;
            CurrentItem = item;
            // TODO: update UI 
            print($"Picked up {item.itemName}");
            return true;
        }

        public ItemDefinition DropItem()
        {
            if(CurrentItem == null) return null;
            var tmp = CurrentItem;
            CurrentItem = null;
            // TODO: update UI
            return tmp;
        }
    }
}
