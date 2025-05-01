using Unity.Cinemachine;
using UnityEngine;

namespace Door
{
    public class RoomTransition : MonoBehaviour
    {
        public CinemachineCamera previousCamera;
        public CinemachineCamera newCamera;
    
        public void SwitchCamera()
        {
            if (previousCamera != null)
                previousCamera.Priority = 0;

            if (newCamera != null)
                newCamera.Priority = 1;
        }
    }
}