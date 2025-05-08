using System.Collections;
using Managers;
using Player;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

public class FreezingRoomEnemy : MonoBehaviour, IEnemy
{
    [Tooltip("Make sure that player stats has the same base time")]
    [SerializeField] private float baseFreezeTime = 10f;

    private float _currentTime;
    private bool _playerInside = false;
    private bool _hasAttacked = false;
    private GameObject _player;
    private Coroutine _freezeCoroutine;
    private int _damage = 1;
    private int _attackInterval = 2;

    private void OnEnable()
    {
        GameEvents.RestartLevel += HandleRestart;
    }

    private void OnDisable()
    {
        GameEvents.RestartLevel -= HandleRestart;
    }

    private void HandleRestart()
    {
        OnRoundStarted(1);
        GameEvents.OnTimerVisibilityChanged?.Invoke(false);
        StopFreezeCoroutine();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var stats = other.GetComponentInParent<PlayerController>()?.stats;
        if (stats == null)
        {
            Debug.LogError("Missing stats!");
            return;
        }

        //_player = other.gameObject;
        _currentTime = stats.TotalFreezeTime;
        _hasAttacked = false;
        _playerInside = true;

        GameEvents.OnTimerVisibilityChanged?.Invoke(true);
        GameEvents.OnTimerUpdated?.Invoke(_currentTime);
        GameEvents.OnFreezeStarted?.Invoke(_currentTime);

        StopFreezeCoroutine();
        _freezeCoroutine = StartCoroutine(FreezeCountdown(other.gameObject));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _playerInside = false;
        GameEvents.OnTimerVisibilityChanged?.Invoke(false);

        StopFreezeCoroutine();

        var stats = other.GetComponent<PlayerController>()?.stats;
        if (stats != null) _currentTime = stats.TotalFreezeTime;
    }
    
    
    /*private IEnumerator FreezeCountdown(GameObject player)
    {
        while (_currentTime > 0f)
        {
            _currentTime -= Time.deltaTime;
            GameEvents.OnTimerUpdated?.Invoke(_currentTime);
            yield return null; 
        }

        _currentTime = 0f;
        GameEvents.OnTimerUpdated?.Invoke(_currentTime);
        GameEvents.OnTimerVisibilityChanged?.Invoke(false);

        if (!_hasAttacked)
        {
            _hasAttacked = true;
            AttackPlayer(player);
        }

        _freezeCoroutine = null;
    }*/
    


    private IEnumerator FreezeCountdown(GameObject player)
    {
        float attackTimer = 0f;

        while (_playerInside)
        {
            _currentTime -= Time.deltaTime;
            GameEvents.OnTimerUpdated?.Invoke(_currentTime);
            if(_currentTime <= 0f)
            {
                if (!_hasAttacked)
                {
                    _hasAttacked = true;
                    AttackPlayer(player);
                }
                attackTimer += Time.deltaTime;
                if (attackTimer >= _attackInterval)
                {
                    attackTimer = 0f;

                    if (player.GetComponentInParent<PlayerHealth>().getHealth() > 0)
                    {
                        AttackPlayer(player);
                    }
                    else
                    {
                        GameEvents.OnTimerVisibilityChanged?.Invoke(false);
                        break;
                    }
                }
            
            }

            yield return null;
        }

        _freezeCoroutine = null;
    }


    private void StopFreezeCoroutine()
    {
        if (_freezeCoroutine != null)
        {
            StopCoroutine(_freezeCoroutine);
            _freezeCoroutine = null;
        }
    }

    public void AttackPlayer(GameObject player)
    {
        var health = player.GetComponentInParent<PlayerHealth>();
        if (health != null)
        {
            //health.TakeDamage(health.getHealth()); // הורג את השחקן
            health.TakeDamage(_damage); 
        }
        else
        {
            Debug.LogWarning("Player has no PlayerHealth!");
        }
    }

    public void OnRoundStarted(int level)
    {
        float newFreezeTime = Mathf.Max(3f, baseFreezeTime - level * 1f);
        _currentTime = newFreezeTime;
    }
}
