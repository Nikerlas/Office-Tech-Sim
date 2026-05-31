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

        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Shopkeeper";
        }

        GameManager.Instance.playerName =
            playerName;

        GameManager.Instance.currentMoney = 0;
        GameManager.Instance.currentDayIndex = 0;
        GameManager.Instance.currentCustomerIndex = 0;
        GameManager.Instance.currentChapterIndex = 0;
        GameManager.Instance.LoadCurrentChapter();
        GameManager.Instance.GenerateTodayCustomers();
        GameManager.Instance.playingChapterIntro = true;
        SceneManager.LoadScene(
            "DialogueScene"
        );
    }

    public void ContinueGame()
    {
        SaveManager.Instance.LoadGame();

        SceneManager.LoadScene(
            "DayStartScene"
        );
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}