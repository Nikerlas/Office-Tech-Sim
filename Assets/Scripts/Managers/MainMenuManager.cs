using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlayMainMenuBGM();
    }

    public void StartGame()
    {
        AudioManager.Instance.PlayButtonClick();

        GameManager.Instance.playerName = "";
        GameManager.Instance.hasCreatedCharacter = false;
        GameManager.Instance.currentMoney = 0;
        GameManager.Instance.currentDayIndex = 0;
        GameManager.Instance.currentCustomerIndex = 0;
        GameManager.Instance.currentChapterIndex = 0;
        GameManager.Instance.LoadCurrentChapter();
        GameManager.Instance.GenerateTodayCustomers();
        GameManager.Instance.playingChapterIntro = true;

        SceneTransitionManager.Instance.FadeToScene("DialogueScene");
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