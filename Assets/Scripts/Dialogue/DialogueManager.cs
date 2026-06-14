using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    PlayerGender selectedGender;
    // public Image portraitImage;
    public TMP_Text speakerText;
    public TMP_Text dialogueText;

    public DialogueData dialogueData;

    public DialogueTyper dialogueTyper;

    int currentIndex = 0;
    int completed = 0;
    int total = 0;

    bool isResultDialogue;

    [Header("Character Setup")]
    public GameObject characterSetupPanel;

    public GameObject genderSelectionPanel;

    public GameObject nameInputPanel;

    public GameObject dialoguePanel;

    public TMP_InputField playerNameInput;

    public PortraitSlot leftPortrait;
    public PortraitSlot centerPortrait;

    public PortraitSlot rightPortrait;

    [Header("Summary Panel")]
    public GameObject summaryPanel;

    public TMP_Text summaryCustomerText;
    public TMP_Text summaryTaskText;
    public TMP_Text summaryRewardText;
    public TMP_Text summaryMoneyText;
    public TMP_Text summaryStoryProgressText;


    void Start()
    {
        Debug.Log(
            "playingChapterIntro: "
            + GameManager.Instance.playingChapterIntro
        );

        Debug.Log(
            "showCharacterSetup: "
            + GameManager.Instance.currentChapter.showCharacterSetup
        );

        Debug.Log(
            "hasCreatedCharacter: "
            + GameManager.Instance.hasCreatedCharacter
        );

        characterSetupPanel.SetActive(false);

        dialoguePanel.SetActive(true);

        Debug.Log("CHECKING CHARACTER SETUP");

        if (GameManager.Instance.playingChapterIntro && GameManager.Instance.currentChapter.showCharacterSetup && !GameManager.Instance.hasCreatedCharacter)
        {
            leftPortrait.gameObject.SetActive(false);
            centerPortrait.gameObject.SetActive(false);
            rightPortrait.gameObject.SetActive(false);

            characterSetupPanel.SetActive(true);

            genderSelectionPanel.SetActive(true);

            nameInputPanel.SetActive(false);

            dialoguePanel.SetActive(false);

            return;
        }
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

        CustomerData customer = GameManager.Instance.GetCurrentTodayCustomer();

        CustomerProgress progress = GameManager.Instance.GetCustomerProgress(customer);

        CustomerJob currentJob = customer.stages[progress.currentStage];

        Debug.Log(
            customer.customerName
            + " Stage "
            + progress.currentStage
        );

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

            SceneTransitionManager.Instance.FadeToScene("DayStartScene");

            return;
        }

        if (GameManager.Instance.playingChapterComplete)
        {
            GameManager.Instance.playingChapterComplete = false;

            SceneTransitionManager.Instance.FadeToScene("ChapterCompleteScene");

            return;
        }

        CustomerData customer =
            GameManager.Instance.GetCurrentTodayCustomer();

        CustomerProgress progress =
            GameManager.Instance.GetCustomerProgress(customer);

        CustomerJob currentJob = customer.stages[progress.currentStage];


        if (!isResultDialogue)
        {
            GameManager.Instance.currentTask =
                currentJob.buildTask;

            SceneTransitionManager.Instance.FadeToScene("AssemblyScene");

            return;
        }


        if (!isResultDialogue)
        {
            GameManager.Instance.currentTask =
                currentJob.buildTask;

            SceneTransitionManager.Instance.FadeToScene("AssemblyScene");

            return;
        }

        if (progress.currentStage < customer.stages.Count - 1)
        {
            progress.currentStage++;

            Debug.Log(
                customer.customerName +
                " Advanced To Stage " +
                progress.currentStage
            );
        }
        else
        {
            progress.isCompleted = true;

            Debug.Log(
                customer.customerName +
                " STORY COMPLETE"
            );
        }

        ShowSummaryPanel(
            customer,
            currentJob
        );

        dialoguePanel.SetActive(false);

        Debug.Log("Customer Finished");
    }

    public void SelectMale()
    {
        selectedGender =
            PlayerGender.Male;

        genderSelectionPanel.SetActive(false);

        nameInputPanel.SetActive(true);
    }

    public void SelectFemale()
    {
        selectedGender =
            PlayerGender.Female;

        genderSelectionPanel.SetActive(false);

        nameInputPanel.SetActive(true);
    }

    public void ConfirmCharacter()
    {
        string playerName =
            playerNameInput.text;

        if (string.IsNullOrWhiteSpace(playerName))
        {
            playerName = "Shopkeeper";
        }

        GameManager.Instance.playerName =
            playerName;

        GameManager.Instance.playerGender =
            selectedGender;

        GameManager.Instance.hasCreatedCharacter =
            true;

        characterSetupPanel.SetActive(false);

        dialoguePanel.SetActive(true);

        dialogueData =
            GameManager.Instance
                .currentChapter
                .chapterIntroDialogue;

        currentIndex = 0;

        ShowLine();
    }

    void ShowSummaryPanel(CustomerData customer, CustomerJob currentJob)
    {
        summaryPanel.SetActive(true);

        summaryCustomerText.text =
            customer.customerName;

        summaryTaskText.text =
            currentJob.buildTask.taskName;

        summaryRewardText.text =
            "+" +
            currentJob.buildTask.rewardMoney;

        summaryMoneyText.text =
            "$" +
            GameManager.Instance.currentMoney;

        foreach (CustomerData c in GameManager.Instance.currentChapter.customerPool)
        {
            if (!c.isStoryCustomer)
            {
                continue;
            }

            total++;

            CustomerProgress progress =
                GameManager.Instance.GetCustomerProgress(c);

            if (progress.isCompleted)
            {
                completed++;
            }
        }

        summaryStoryProgressText.text =
            completed + " / " + total;
    }

    public void ContinueAfterSummary()
    {
        summaryPanel.SetActive(false);

        GameManager.Instance.NextCustomer();

        if (GameManager.Instance.dayFinished)
        {
            SceneTransitionManager.Instance
                .FadeToScene("DayEndScene");
        }
        else
        {
            SceneTransitionManager.Instance
                .FadeToScene("DialogueScene");
        }
    }
}