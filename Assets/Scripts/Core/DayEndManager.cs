using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DayEndManager : MonoBehaviour
{
    public PortraitSlot leftPortrait;
    public PortraitSlot centerPortrait;
    public PortraitSlot rightPortrait;
    public GameObject dialoguePanel;
    public GameObject summaryPanel;

    public TMP_Text currentMoneyText;
    public TMP_Text targetMoneyText;
    public TMP_Text speakerText;
    public TMP_Text dialogueText;
    public DialogueTyper dialogueTyper;

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
            GameManager.Instance.playingChapterComplete =
                true;

            SceneTransitionManager.Instance.FadeToScene("DialogueScene");
        }
        else
        {
            SceneTransitionManager.Instance.FadeToScene("DayStartScene");
        }
    }

    public void SaveGame()
    {
        SaveManager.Instance.SaveGame();
    }
}