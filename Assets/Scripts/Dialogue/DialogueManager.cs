using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    PlayerGender selectedGender;
    [Header("Character Setup")]
    public GameObject characterSetupPanel;

    public GameObject genderSelectionPanel;

    public GameObject nameInputPanel;

    public GameObject dialoguePanel;

    public TMP_InputField playerNameInput;

    public Image portraitImage;
    public TMP_Text speakerText;
    public TMP_Text dialogueText;

    public DialogueData dialogueData;

    [Header("Male Portraits")]
    public Sprite maleNeutral;
    public Sprite maleHappy;
    public Sprite maleAngry;
    public Sprite maleShocked;
    public Sprite maleConfused;

    [Header("Female Portraits")]
    public Sprite femaleNeutral;
    public Sprite femaleHappy;
    public Sprite femaleAngry;
    public Sprite femaleShocked;
    public Sprite femaleConfused;

    int currentIndex = 0;

    bool isResultDialogue;

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
            Debug.Log("SHOW CHARACTER SETUP");
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

    Sprite GetPlayerPortrait(PlayerExpression expression)
    {
        bool isMale =
            GameManager.Instance.playerGender
            == PlayerGender.Male;

        if (isMale)
        {
            switch (expression)
            {
                case PlayerExpression.Happy:
                    return maleHappy;

                case PlayerExpression.Angry:
                    return maleAngry;

                case PlayerExpression.Shocked:
                    return maleShocked;

                case PlayerExpression.Confused:
                    return maleConfused;

                default:
                    return maleNeutral;
            }
        }

        switch (expression)
        {
            case PlayerExpression.Happy:
                return femaleHappy;

            case PlayerExpression.Angry:
                return femaleAngry;

            case PlayerExpression.Shocked:
                return femaleShocked;

            case PlayerExpression.Confused:
                return femaleConfused;

            default:
                return femaleNeutral;
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

        if (line.usePlayerPortrait)
        {
            portraitImage.sprite =
                GetPlayerPortrait(
                    line.playerExpression
                );
        }
        else
        {
            portraitImage.sprite =
                line.portrait;
        }
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

            SceneTransitionManager.Instance.FadeToScene("DayStartScene");

            return;
        }

        if (GameManager.Instance.playingChapterComplete)
        {
            GameManager.Instance.playingChapterComplete = false;

            SceneTransitionManager.Instance.FadeToScene("ChapterCompleteScene");

            return;
        }

        CustomerJob currentJob =
            GameManager.Instance.GetCurrentTodayCustomer();

        if (!isResultDialogue)
        {
            GameManager.Instance.currentTask =
                currentJob.buildTask;

            SceneTransitionManager.Instance.FadeToScene("AssemblyScene");

            return;
        }

        GameManager.Instance.NextCustomer();

        if (GameManager.Instance.dayFinished)
        {
            SceneTransitionManager.Instance.FadeToScene("DayEndScene");
        }
        else
        {
            SceneTransitionManager.Instance.FadeToScene("DialogueScene");
        }

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
}