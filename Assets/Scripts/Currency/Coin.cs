using Managers;
using UnityEngine;

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
                CurrencyManager.Instance.GetCoinSpawner().OnCoinCollected();
            }
        }
    }
}