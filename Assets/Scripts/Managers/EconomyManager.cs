using TMPro;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;
    public TMP_Text moneyText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Update UI saat pertama kali scene dimuat agar angkanya sinkron
        UpdateUI();
    }

    // Pastikan fungsi AddMoney ini ada dan menerima parameter (int amount)
    public void AddMoney(int amount)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.currentMoney += amount;
            UpdateUI();
            
            // Jaga-jaga jika ChapterManager ada di scene ini
            if (ChapterManager.Instance != null)
            {
                ChapterManager.Instance.CheckChapterUnlock();
            }
        }
    }

    public void UpdateUI()
    {
        if (moneyText != null && GameManager.Instance != null)
        {
            moneyText.text = "$ " + GameManager.Instance.currentMoney;
        }
    }
}