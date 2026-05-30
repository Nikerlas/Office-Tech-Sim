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

    DialogueData dialogueData;

    void Start()
    {
        summaryPanel.SetActive(false);
        dialoguePanel.SetActive(true);

        DayData currentDay =
            GameManager.Instance.currentChapter
            .days[GameManager.Instance.currentDayIndex];

        dialogueData =
            currentDay.endDayDialogue;

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
            ShowSummary();

            return;
        }

        ShowLine();
    }

    void ShowSummary()
    {
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

        SceneManager.LoadScene("DayStartScene");
    }
}