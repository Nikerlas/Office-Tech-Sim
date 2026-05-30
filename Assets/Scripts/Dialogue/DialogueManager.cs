using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text speakerText;
    public TMP_Text dialogueText;

    public DialogueData dialogueData;

    int currentIndex = 0;

    bool isResultDialogue;

    void Start()
    {
        CustomerJob currentJob =
            GameManager.Instance.GetCurrentCustomerJob();

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
            EndDialogue();

            return;
        }

        ShowLine();
    }

    void EndDialogue()
    {
        CustomerJob currentJob =
            GameManager.Instance.GetCurrentCustomerJob();

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