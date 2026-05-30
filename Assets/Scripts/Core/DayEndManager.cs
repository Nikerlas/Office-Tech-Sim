using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayEndManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public GameObject summaryPanel;

    public TMP_Text currentMoneyText;
    public TMP_Text targetMoneyText;
    public TMP_Text speakerText;
    public TMP_Text dialogueText;

    int currentIndex = 0;

    bool showingStoryEvent;
    StoryEventData currentStoryEvent;

    DialogueData dialogueData;

    void Start()
    {
        currentStoryEvent = GameManager.Instance.GetTodayStoryEvent();

        summaryPanel.SetActive(false);
        dialoguePanel.SetActive(true);

        var pool = GameManager.Instance.currentChapter.endDayDialogues;

        int randomIndex =
            Random.Range(0, pool.Count);

        dialogueData =
            pool[randomIndex];

        ShowLine();
    }

    void ShowLine()
    {
        DialogueLine line =
            dialogueData.lines[currentIndex];

        speakerText.text =
            line.speakerName.Replace(
                "{PLAYER}",
                GameManager.Instance.playerName
            );

        dialogueText.text =
            line.dialogueText.Replace(
                "{PLAYER}",
                GameManager.Instance.playerName
            );
    }

    public void NextLine()
    {
        currentIndex++;

        if (currentIndex >= dialogueData.lines.Count)
        {
            EndCurrentDialogue();

            return;
        }

        ShowLine();
    }

    void EndCurrentDialogue()
    {
        if (!showingStoryEvent &&
           currentStoryEvent != null)
        {
            showingStoryEvent = true;

            dialogueData =
                currentStoryEvent.eventDialogue;

            currentIndex = 0;

            ShowLine();

            return;
        }

        if (currentStoryEvent != null && currentStoryEvent.forcedCustomer != null)
        {
            GameManager.Instance.SetForcedCustomer(
                currentStoryEvent.forcedCustomer
            );
        }

        ShowSummary();
    }

    void ShowSummary()
    {
        if (dialogueData.nextForcedCustomer != null)
        {
            GameManager.Instance.SetForcedCustomer(
                dialogueData.nextForcedCustomer
            );
        }

        dialoguePanel.SetActive(false);

        summaryPanel.SetActive(true);

        currentMoneyText.text =
            "$" + GameManager.Instance.currentMoney;

        targetMoneyText.text =
            "$" + GameManager.Instance.currentChapter.targetMoney;
    }

    public void Sleep()
    {
        GameManager.Instance.SleepAndProgress();

        if (GameManager.Instance.chapterComplete)
        {
            SceneManager.LoadScene(
                "ChapterCompleteScene"
            );
        }
        else
        {
            SceneManager.LoadScene(
                "DayStartScene"
            );
        }
    }

    public void SaveGame()
    {
        SaveManager.Instance.SaveGame();
    }
}