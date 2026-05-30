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

    public void AddMoney(int amount)
    {
        GameManager.Instance.currentMoney += amount;

        UpdateUI();

        ChapterManager.Instance.CheckChapterUnlock();
    }

    void UpdateUI()
    {
        moneyText.text = "$ " + GameManager.Instance.currentMoney;
    }
}