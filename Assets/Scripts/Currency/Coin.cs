using System;
using Managers;
using UnityEngine;
using Utils;

namespace Currency
{
    public class Coin : MonoBehaviour
    {    
        [SerializeField] private int moneyWorth = 10;
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                CurrencyManager.Instance.AddMoney(moneyWorth);
                GameEvents.OnCoinCollected?.Invoke();
                //CurrencyManager.Instance.GetCoinSpawner().OnCoinCollected();
                
                FindObjectOfType<AnimatedCoinPickup>()?.AnimateCoin(transform.position);
                CoinPool.Instance.ReturnCoin(gameObject);
            }
        }
    }
}