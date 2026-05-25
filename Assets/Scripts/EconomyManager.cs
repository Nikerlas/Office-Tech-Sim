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
        // Pastikan UI menampilkan uang terbaru dari GameManager saat scene dimulai
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (moneyText != null && GameManager.Instance != null)
        {
            moneyText.text = "$ " + GameManager.Instance.currentMoney;
        }
    }
}