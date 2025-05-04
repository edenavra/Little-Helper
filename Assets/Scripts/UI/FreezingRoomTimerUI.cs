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
        bool isNegative = time < 0f;
        float displayTime = Mathf.Abs(time);
        
        int minutes = Mathf.FloorToInt(displayTime / 60f);
        int seconds = Mathf.FloorToInt(displayTime % 60f);
        int hundredths = Mathf.FloorToInt((displayTime * 100f) % 100f);
        
        string prefix = isNegative ? "-" : "";
        timerText.text = $"{prefix}{minutes:00}:{seconds:00}:{hundredths:00}";
        if (isNegative)
        {
            timerText.color = Color.red;
        }
        else
        {
            timerText.color = Color.white; 
        }
    }

    private void ToggleTimerUI(bool isVisible)
    {
        if (timerText != null)
            timerText.gameObject.SetActive(isVisible);
    }
}