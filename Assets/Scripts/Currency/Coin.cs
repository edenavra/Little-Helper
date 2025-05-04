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
                CurrencyManager.Instance.AddMoney(20);
                GameEvents.OnCoinCollected?.Invoke();
                //CurrencyManager.Instance.GetCoinSpawner().OnCoinCollected();
                
                FindObjectOfType<AnimatedCoinPickup>()?.AnimateCoin(transform.position);
                CoinPool.Instance.ReturnCoin(gameObject);
            }
        }
    }
}