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
        // [SerializeField] private List<ItemDefinition> recipeItems;
        // [SerializeField] private List<ItemDefinition> trashItems;
        [SerializeField] private WorldGenerator worldGenerator;
        [SerializeField] internal GameObject playerObject;
        [SerializeField] private CurrencyManager currencyManager;
        //[SerializeField] private GameObject upgradePanel;
        public List<Room> Rooms { get; private set;}
        
        [SerializeField] private WorldConfig worldConfig;
        
        // public List<ItemDefinition> RecipeItems => recipeItems;
        // public List<ItemDefinition> TrashItems => trashItems;
        
      //  public List<ItemDefinition> RecipeItems => worldConfig.
        public GameObject PlayerObject => playerObject; 
    
        private int _currentRound;

        public int CurrentRound => _currentRound;
        
        public Dictionary<UpgradeType, int> PurchasedUpgrades { get; private set; } = new();
        
       
        private bool isTutorialCompleted = true; //TODO: change the default to false after creating tutorial

        private void OnEnable()
        {
            GameEvents.PlayerDied += HandlePlayerDied;
        }
        private void OnDisable()
        {
            GameEvents.PlayerDied -= HandlePlayerDied;
        }

        private void Awake()
        {
            //Rooms = new List<Room>();
        }

        private void Start()
        {
            if (SceneManager.GetActiveScene().name == "SandBox")
            {
                StartRun();
            }
        }
        
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
                if (room != null)
                    Destroy(room.gameObject);
            }
            Rooms.Clear();
            GenerateWorld();
            
        }
        
        private void StartRun()
        {
            _currentRound = 0;
            print("generating world");
            GenerateWorld();
            RoundManager.Instance.SetRooms(Rooms);
            GameEvents.StartQuest.Invoke();
            StartNextRound();
        }
        
        private void GenerateWorld()
        { 
            this.Rooms = worldGenerator.GenerateWorld();
            ItemPlacer.PopulateContainers(worldConfig.recipeItems, Rooms);
            ItemPlacer.PopulateContainers(worldConfig.trashItems, Rooms);
            currencyManager.GetCoinSpawner().SetRooms(Rooms);
            currencyManager.GetCoinSpawner().InitialSpawn();
        }
        
        private void StartNextRound()
        {
            _currentRound++;
            foreach (var room in Rooms)
            {
                room.OnRoundStarted(_currentRound);
            }
        }
        
        public void HandlePlayerDied()
        {
            Debug.Log("Player Failed!");
            //upgradePanel.SetActive(true);
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
                SceneManager.LoadScene("SandBox");
        }
        
    }
}
