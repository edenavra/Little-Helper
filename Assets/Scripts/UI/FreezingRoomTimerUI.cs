using TMPro;
using UnityEngine;
using Utils;

public class FreezingRoomTimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText; 
    //[SerializeField] private FreezingRoomEnemy freezingRoomEnemy;

    private void Awake()
    {
        if (timerText != null)
            timerText.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.OnTimerUpdated += UpdateTimer;
        GameEvents.OnTimerVisibilityChanged += ToggleTimerUI;
    }

    private void OnDisable()
    {
        GameEvents.OnTimerUpdated -= UpdateTimer;
        GameEvents.OnTimerVisibilityChanged -= ToggleTimerUI;
    }

    private void UpdateTimer(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int hundredths = Mathf.FloorToInt((time * 100f) % 100f);

        timerText.text = $"{minutes:00}:{seconds:00}:{hundredths:00}";
    }

    private void ToggleTimerUI(bool isVisible)
    {
        if (timerText != null)
            timerText.gameObject.SetActive(isVisible);
    }
}