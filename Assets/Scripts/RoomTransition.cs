using System;
using UnityEngine;
using Unity.Cinemachine;


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