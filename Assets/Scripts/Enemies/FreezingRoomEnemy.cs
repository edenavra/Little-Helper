using System.Collections;
using Managers;
using Player;
using UnityEngine;
using Utils;

public class FreezingRoomEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] private float freezeTime = 10f;

    private float currentTime;
    private bool playerInside = false;
    private bool hasAttacked = false;
    private GameObject _player;
    private Coroutine freezeCoroutine;

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
        currentTime = stats.TotalFreezeTime;
        hasAttacked = false;
        playerInside = true;

        GameEvents.OnTimerVisibilityChanged?.Invoke(true);
        GameEvents.OnTimerUpdated?.Invoke(currentTime);

        StopFreezeCoroutine();
        freezeCoroutine = StartCoroutine(FreezeCountdown(other.gameObject));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        GameEvents.OnTimerVisibilityChanged?.Invoke(false);

        StopFreezeCoroutine();

        var stats = other.GetComponent<PlayerController>()?.stats;
        currentTime = stats != null ? stats.TotalFreezeTime : freezeTime;
    }

    private IEnumerator FreezeCountdown(GameObject player)
    {
        while (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            GameEvents.OnTimerUpdated?.Invoke(currentTime);
            yield return null; 
        }

        currentTime = 0f;
        GameEvents.OnTimerUpdated?.Invoke(currentTime);
        GameEvents.OnTimerVisibilityChanged?.Invoke(false);

        if (!hasAttacked)
        {
            hasAttacked = true;
            AttackPlayer(player);
        }

        freezeCoroutine = null;
    }

    private void StopFreezeCoroutine()
    {
        if (freezeCoroutine != null)
        {
            StopCoroutine(freezeCoroutine);
            freezeCoroutine = null;
        }
    }

    public void AttackPlayer(GameObject player)
    {
        var health = player.GetComponentInParent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(health.getHealth()); // הורג את השחקן
        }
        else
        {
            Debug.LogWarning("Player has no PlayerHealth!");
        }
    }

    public void OnRoundStarted(int level)
    {
        float newFreezeTime = Mathf.Max(3f, freezeTime - level * 1f);
        currentTime = newFreezeTime;
    }
}
