using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DayStartManager : MonoBehaviour
{
    public PortraitSlot leftPortrait;
    public PortraitSlot centerPortrait;
    public PortraitSlot rightPortrait;
    public TMP_Text speakerText;
    public TMP_Text dialogueText;
    public DialogueTyper dialogueTyper;

    int currentIndex = 0;

    DialogueData dialogueData;

    void Start()
    {
        AudioManager.Instance.PlayGameplayBGM();

        var pool = GameManager.Instance.currentChapter.startDayDialogues;

        int randomIndex =
            Random.Range(0, pool.Count);

        dialogueData =
            pool[randomIndex];

        ShowLine();
    }

    void Update()
    {
        if (
            Input.GetKeyDown(
                KeyCode.Space
            )
        )
        {
            NextLine();
        }

        if (
            Input.GetKeyDown(
                KeyCode.Return
            )
        )
        {
            NextLine();
        }
    }

    public void OnDialogueClicked()
    {
        NextLine();
    }

    void ShowLine()
    {
        DialoguePresenter.ShowLine(
            dialogueData.lines[currentIndex],
            speakerText,
            dialogueText,
            leftPortrait,
            rightPortrait,
            centerPortrait
        );

        dialogueTyper.StartTyping(DialoguePresenter.BuildDialogueText(dialogueData.lines[currentIndex]));
    }

    public void NextLine()
    {
        if (dialogueTyper.TryCompleteTyping())
        {
            return;
        }

        currentIndex++;

        if (currentIndex >= dialogueData.lines.Count)
        {
            SceneTransitionManager.Instance.FadeToScene("DialogueScene");

            return;
        }

        ShowLine();
    }
}