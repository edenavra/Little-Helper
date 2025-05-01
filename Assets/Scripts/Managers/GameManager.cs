using System;
using System.Collections.Generic;
using Item;
using Rooms;
using Scriptable_Objects;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using Player;

namespace Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [Header("Run Setup")] 
        [SerializeField] private int rounds = 7;
        [SerializeField] private List<ItemDefinition> recipeItems;
        [SerializeField] private List<ItemDefinition> trashItems;
        [SerializeField] private WorldGenerator worldGenerator;
        [SerializeField] internal GameObject playerObject;
        [SerializeField] private CurrencyManager currencyManager;
        
        public  List<Room> Rooms { get; private set;}
        
        public List<ItemDefinition> RecipeItems => recipeItems;
        public List<ItemDefinition> TrashItems => trashItems;
        public GameObject PlayerObject => playerObject; 
    
        private int _currentRound;

        public int CurrentRound => _currentRound;
        
        public Dictionary<UpgradeType, int> PurchasedUpgrades { get; private set; } = new();
       
        private bool isTutorialCompleted = true; //TODO: change the default to false after creating tutorial


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                GameEvents.RestartLevel.Invoke();
                RestartLevel();
            }
        }

        private void RestartLevel()
        {
            
            //delete current rooms 
            foreach (Room room in Rooms)
            {
                Destroy(room);
            }
            GenerateWorld();
            
        }

        private void Awake()
        {
            Rooms = new List<Room>();
        }

        private void Start()
        {
            if (SceneManager.GetActiveScene().name == "SandBox")
            {
                StartRun();
            }
        }
        
        private void StartRun()
        {
            _currentRound = 1;
            print("generating world");
            GenerateWorld();
            currencyManager.GetCoinSpawner().InitialSpawn();
            print("starting next round");
            StartNextRound();
        }
        
        private void GenerateWorld()
        {
            Rooms = worldGenerator.GenerateWorld();
            ItemPlacer.PopulateContainers(recipeItems);
            ItemPlacer.PopulateContainers(trashItems);
        }
        
        private void StartNextRound()
        {
            _currentRound++;
            foreach (var room in Rooms)
            {
                room.OnRoundStarted(_currentRound);
            }
        }
        
        public void PlayerFailed()
        {
            Debug.Log("Player Failed!");
            //SoundManager.Instance.PlayGameOver();
        }
        
        public void PlayerSucceeded()
        {
            Debug.Log("Player Won!");
            CurrencyManager.Instance.ResetMoney();
            SceneManager.LoadScene("VictoryScene");
        }
        
        public void FinishRun()
        {
            SceneManager.LoadScene("Start");
        }
        
        public void ContinueAfterUpgrade()
        {
            SceneManager.LoadScene("GameScene");
        }
        
        public void RegisterUpgrade(UpgradeType type)
        {
            if (!PurchasedUpgrades.ContainsKey(type))
            {
                PurchasedUpgrades[type] = 0;
            }
            PurchasedUpgrades[type]++;
        }
        
        public void OnStartGameButtonPressed()
        {
            if (isTutorialCompleted)
                SceneManager.LoadScene("GameScene");
        }
        
    }
}
