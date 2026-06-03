using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DayStartManager : MonoBehaviour
{
    public PortraitSlot leftPortrait;
    public PortraitSlot rightPortrait;
    public TMP_Text speakerText;
    public TMP_Text dialogueText;

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

    void ShowLine()
    {
        DialoguePresenter.ShowLine(
            dialogueData.lines[currentIndex],
            speakerText,
            dialogueText,
            leftPortrait,
            rightPortrait
        );
    }

    public void NextLine()
    {
        currentIndex++;

        if (currentIndex >= dialogueData.lines.Count)
        {
            SceneTransitionManager.Instance.FadeToScene("DialogueScene");

            return;
        }

        ShowLine();
    }
}