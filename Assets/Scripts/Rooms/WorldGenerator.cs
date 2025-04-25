using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Rooms
{
    
    public class WorldGenerator : MonoBehaviour
    {
        [SerializeField] private WorldConfig config;
        [SerializeField] private Transform roomsParent;
        
        private GameObject _kitchen, _roomLeft, _roomRight, _roomThird;
        
        public List<Room> GenerateWorld()
        {
            _kitchen = Instantiate(config.kitchenPrefab,Vector2.zero,Quaternion.identity,roomsParent);
            var types = new List<RoomType> { RoomType.Pantry, RoomType.Freezer, RoomType.Garden };
            RoomType rightRoomType = PopRandom(types);
            RoomType leftRoomType = PopRandom(types);
            RoomType thirdRoomType = types[0];


            _roomRight = InstantiateRoom(rightRoomType, Vector2.right);
            _roomLeft = InstantiateRoom(leftRoomType, Vector2.left);

            bool attachedRight = Random.value < 0.5f;
            Vector2 dir = attachedRight ? Vector2.right : Vector2.left;
            _roomThird = InstantiateRoom(thirdRoomType, dir * 2f);
            
            Connect(_kitchen, DoorSide.Right, _roomRight, DoorSide.Left);
            Connect(_kitchen, DoorSide.Left, _roomLeft, DoorSide.Right);
            if (attachedRight)
            {
                Connect (_roomRight, DoorSide.Right, _roomThird, DoorSide.Left);
                DisableDoor(_roomLeft, DoorSide.Left);
                DisableDoor(_roomThird, DoorSide.Right);
            }
            else
            {
                Connect(_roomLeft,DoorSide.Left, _roomThird, DoorSide.Right);
                DisableDoor(_roomRight, DoorSide.Right);
                DisableDoor(_roomThird, DoorSide.Left);
            }
            
            return new List<Room> { _roomRight.GetComponent<Room>(), _roomLeft.GetComponent<Room>(), _roomThird.GetComponent<Room>() };
        }
        
        private void Connect(GameObject fromRoom, DoorSide fromSide, GameObject toRoom, DoorSide toSide)
        {
            var doorFrom = fromRoom.transform.Find($"Doors/{fromSide} Door");
            var doorTo = toRoom.transform.Find($"Doors/{toSide} Door");
            
            if (doorFrom == null || doorTo == null)
            {
                Debug.LogError($"Missing door: {fromSide} in {fromRoom.name} or {toSide} in {toRoom.name}");
            }
            
            var doorFromTrigger = doorFrom.GetComponent<DoorTrigger>();
            doorFromTrigger.targetPosition = doorTo.Find("EntryPoint");
            
            var doorToTrigger = doorTo.GetComponent<DoorTrigger>();
            doorToTrigger.targetPosition = doorFrom.Find("EntryPoint");
            
            var rt = doorFrom.GetComponent<RoomTransition>();
            rt.previousCamera = fromRoom.GetComponentInChildren<CinemachineCamera>();
            rt.newCamera = toRoom.GetComponentInChildren<CinemachineCamera>();
        }
        
        private void DisableDoor(GameObject room, DoorSide side)
        {
            var door = room.transform.Find(side + "Door");
            if(door != null) door.gameObject.SetActive(false);
        }

        private GameObject InstantiateRoom(RoomType type, Vector2 dir)
        {
            GameObject roomPrefab;
            switch (type)
            {
                case RoomType.Pantry:
                    roomPrefab = RandomFrom(config.pantryLayouts);
                    break;
                case RoomType.Freezer:
                    roomPrefab = RandomFrom(config.freezerLayouts);
                    break;
                case RoomType.Garden:
                    roomPrefab = RandomFrom(config.gardenLayouts);
                    break;
                default:
                    return null;
            }
            return Instantiate(roomPrefab, dir * config.roomSpacing, Quaternion.identity, roomsParent);
            
        }
        
        private GameObject RandomFrom(List<GameObject> list) => list[Random.Range(0, list.Count)];

        private static RoomType PopRandom(List<RoomType> types)
        {
            var i = Random.Range(0, types.Count);
            var roomType = types[i];
            types.RemoveAt(i);
            return roomType;
        }
    }
}


