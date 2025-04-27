using UnityEngine;
using TMPro;

public class FreezingRoomEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] private float freezeTime = 10f; 
    private TextMeshProUGUI timerText;

    private float currentTime;
    private bool playerInside = false;
    private bool hasAttacked = false;
    private GameObject player;

    private void Start()
    {
        var go = GameObject.FindWithTag("FreezeTimer");
        timerText = go.GetComponent<TextMeshProUGUI>();
        timerText.gameObject.SetActive(false);
        currentTime = freezeTime;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (playerInside && !hasAttacked)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerUI();

            if (currentTime <= 0f)
            {
                currentTime = 0f;
                UpdateTimerUI();
                AttackPlayer(player);
                hasAttacked = true;
            }
        }
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        int hundredths = Mathf.FloorToInt((currentTime * 100f) % 100f);

        timerText.text = $"{minutes:00}:{seconds:00}:{hundredths:00}";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            currentTime = freezeTime;
            hasAttacked = false;
            timerText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            timerText.gameObject.SetActive(false);
            currentTime = freezeTime;
        }
    }

    public void AttackPlayer(GameObject player)
    {
        var health = player.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(health.getHealth()); 
        }
    }

    public void OnRoundStarted(int level)
    {
        float newFreezeTime = Mathf.Max(3f, freezeTime - level * 1f);
        currentTime = newFreezeTime;
    }
}
