using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayStartManager : MonoBehaviour
{
    public TMP_Text speakerText;
    public TMP_Text dialogueText;

    int currentIndex = 0;

    DialogueData dialogueData;

    void Start()
    {
        DayData currentDay =
            GameManager.Instance.currentChapter
            .days[GameManager.Instance.currentDayIndex];

        dialogueData =
            currentDay.startDayDialogue;

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
            SceneManager.LoadScene("DialogueScene");

            return;
        }

        ShowLine();
    }
}