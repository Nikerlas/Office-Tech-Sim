using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Image portraitImage;
    public TMP_Text speakerText;
    public TMP_Text dialogueText;

    public DialogueData dialogueData;

    int currentIndex = 0;

    bool isResultDialogue;

    void Start()
    {
        if (GameManager.Instance.playingChapterIntro)
        {
            dialogueData =
                GameManager.Instance
                    .currentChapter
                    .chapterIntroDialogue;

            ShowLine();

            return;
        }

        if (GameManager.Instance.playingChapterComplete)
        {
            dialogueData =
                GameManager.Instance
                    .currentChapter
                    .chapterCompleteDialogue;

            ShowLine();

            return;
        }

        CustomerJob currentJob =
            GameManager.Instance.GetCurrentTodayCustomer();

        if (GameManager.Instance.returningFromAssembly)
        {
            dialogueData =
                currentJob.resultDialogue;

            isResultDialogue = true;

            GameManager.Instance.returningFromAssembly = false;
        }
        else
        {
            dialogueData =
                currentJob.introDialogue;

            isResultDialogue = false;
        }

        ShowLine();

        if (GameManager.Instance.dayFinished)
        {
            Debug.Log("SHOW DAY COMPLETE SCREEN");

            return;
        }
    }

    public void ShowLine()
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

        portraitImage.sprite =
            line.portrait;
    }

    public void NextLine()
    {
        currentIndex++;

        if (currentIndex >= dialogueData.lines.Count)
        {
            EndDialogue();

            return;
        }

        ShowLine();
    }

    void EndDialogue()
    {
        if (GameManager.Instance.playingChapterIntro)
        {
            GameManager.Instance.playingChapterIntro = false;

            SceneManager.LoadScene(
                "DayStartScene"
            );

            return;
        }

        if (GameManager.Instance.playingChapterComplete)
        {
            GameManager.Instance.playingChapterComplete = false;

            SceneManager.LoadScene(
                "ChapterCompleteScene"
            );

            return;
        }

        CustomerJob currentJob =
            GameManager.Instance.GetCurrentTodayCustomer();

        if (!isResultDialogue)
        {
            GameManager.Instance.currentTask =
                currentJob.buildTask;

            SceneManager.LoadScene("AssemblyScene");

            return;
        }

        GameManager.Instance.NextCustomer();

        if (GameManager.Instance.dayFinished)
        {
            SceneManager.LoadScene("DayEndScene");
        }
        else
        {
            SceneManager.LoadScene("DialogueScene");
        }

        Debug.Log("Customer Finished");
    }
}