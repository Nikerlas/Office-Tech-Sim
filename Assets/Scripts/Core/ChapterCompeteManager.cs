using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterCompleteManager : MonoBehaviour
{
    public void Continue()
    {
        GameManager.Instance.chapterComplete = false;

        GameManager.Instance.currentChapterIndex++;

        GameManager.Instance.currentDayIndex = 0;

        GameManager.Instance.currentCustomerIndex = 0;

        GameManager.Instance.LoadCurrentChapter();

        GameManager.Instance.GenerateTodayCustomers();

        GameManager.Instance.playingChapterIntro = true;

        SceneManager.LoadScene("DialogueScene");
    }
}