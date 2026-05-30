using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public TMP_InputField nameInput;

    public void StartGame()
    {
        string playerName =
            nameInput.text;

        if(string.IsNullOrEmpty(playerName))
        {
            playerName = "Shopkeeper";
        }

        GameManager.Instance.playerName =
            playerName;

        GameManager.Instance.currentMoney = 0;
        GameManager.Instance.currentDayIndex = 0;
        GameManager.Instance.currentCustomerIndex = 0;

        SceneManager.LoadScene("DayStartScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}