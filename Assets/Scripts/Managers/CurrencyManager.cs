using Currency;
using UnityEngine;
using Utils;

namespace Managers
{
    public class CurrencyManager : MonoSingleton<CurrencyManager>
    {
        [SerializeField] private CoinSpawner coinSpawner; 
        
        private int _currentMoney;
        
        public void AddMoney(int amount)
        {
            _currentMoney += amount;
            Debug.Log("Money: " + _currentMoney);
            
            //TODO: add money to UI
        }

        public int GetMoney()
        {
            return _currentMoney;
        }

        public void ResetMoney()
        {
            _currentMoney = 0;
        }
        
        public CoinSpawner GetCoinSpawner()
        {
            return coinSpawner;
        }
        
        public bool CanAfford(int price)
        {
            return _currentMoney >= price;
        }

        public void SpendMoney(int amount)
        {
            _currentMoney -= amount;
        }

    }
}