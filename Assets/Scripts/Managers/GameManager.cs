using System.Collections.Generic;
using Item;
using Rooms;
using Scriptable_Objects;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

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
        public  List<Room> Rooms { get; private set;}
        
        public List<ItemDefinition> PlacedItems => recipeItems;
        public List<ItemDefinition> TrashItems => trashItems;
        public GameObject PlayerObject => playerObject; 
    
        private int _currentRound;

        public int CurrentRound => _currentRound;

        private void Awake()
        {
            Rooms = new List<Room>();
            /*Rooms = worldGenerator.GenerateWorld();
            ItemPlacer.PopulateContainers(recipeItems);
            ItemPlacer.PopulateContainers(TrashItems);*/
        }

        private void Start()
        {
            if (SceneManager.GetActiveScene().name == "GameScene")
            {
                StartRun();
            }
        }
        
        public void StartRun()
        {
            _currentRound = 1;
            GenerateWorld();
            StartNextRound();
        }
        
        private void GenerateWorld()
        {
            Rooms = worldGenerator.GenerateWorld();
            ItemPlacer.PopulateContainers(recipeItems);
            ItemPlacer.PopulateContainers(trashItems);
        }
        
        public void StartNextRound()
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
            SceneManager.LoadScene("UpgradeScene");
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
    }
}
