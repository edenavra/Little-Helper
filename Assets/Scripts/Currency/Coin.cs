using System;
using Managers;
using UnityEngine;
using Utils;

namespace Currency
{
    public class Coin : MonoBehaviour
    {    

        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                CurrencyManager.Instance.AddMoney(1);
                CoinPool.Instance.ReturnCoin(gameObject);
                GameEvents.OnCoinCollected?.Invoke();
                //CurrencyManager.Instance.GetCoinSpawner().OnCoinCollected();
            }
        }
    }
}