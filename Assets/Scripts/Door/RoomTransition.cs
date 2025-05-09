using Managers;
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
            {
                previousCamera.LookAt = null;
                previousCamera.Follow = null;
                previousCamera.Priority = 0;
            }

            if (newCamera != null)
            {
                newCamera.LookAt = GameManager.Instance.PlayerObject.transform;
                newCamera.Follow = GameManager.Instance.PlayerObject.transform;
                newCamera.Priority = 1;
            }
        }
    }
}