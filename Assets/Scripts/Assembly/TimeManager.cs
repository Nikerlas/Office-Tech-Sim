using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("Timer Settings")]
    [SerializeField] float remainingTime = 60f; // 1 menit
    public bool timerIsRunning = true;

    [Header("UI Component")]
    public TMP_Text timerText;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!timerIsRunning) return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else
        {
            remainingTime = 0;
            timerIsRunning = false;
            
            Debug.Log("Waktu Habis! Memicu Otomatis Check...");
            
            if (TaskManager.Instance != null)
            {
                TaskManager.Instance.CheckCurrentTask();
            }
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        
        if (timerText != null)
        {
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            
            if (remainingTime <= 10f)
            {
                timerText.color = Color.red;
            }
        }
    }

    // FUNGSI BARU: Untuk memberikan data sisa waktu ke TaskManager
    public float GetRemainingTime()
    {
        return remainingTime;
    }
}