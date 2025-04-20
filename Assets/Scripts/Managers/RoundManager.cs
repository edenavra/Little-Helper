using System;
using System.Collections.Generic;
using Grandma;
using Rooms;
using Scriptable_Objects;
using UnityEngine;
using Utils;

namespace Managers
{
    public class RoundManager : MonoSingleton<RoundManager>
    {
        [SerializeField] private GameManager gameManager; 
        [SerializeField] private GrandmaQuestGiver questGiver;
        [SerializeField] private List<Room> rooms; 

        private int _currentRound;

        
        private void Start()
        {
            StartNextRound();
        }

        private void OnEnable()
        {
            questGiver.OnItemDelivered += StartNextRound;
        }

        private void OnDisable()
        {
            questGiver.OnItemDelivered -= StartNextRound;
        }

        private void StartNextRound()
        {
            _currentRound++;
            //notify all rooms about next round (to increase difficultly)
            foreach (var room in rooms) room.OnRoundStarted(_currentRound);
        }
    }
}
