using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class DialoguePresenter
{
    static Sprite GetPlayerPortrait(
    PlayerExpression expression
)
    {
        DialogueManager manager =
            Object.FindFirstObjectByType<DialogueManager>();

        if (manager == null)
        {
            return null;
        }

        bool isMale =
            GameManager.Instance.playerGender
            == PlayerGender.Male;

        if (isMale)
        {
            switch (expression)
            {
                case PlayerExpression.Happy:
                    return manager.maleHappy;

                case PlayerExpression.Angry:
                    return manager.maleAngry;

                case PlayerExpression.Shocked:
                    return manager.maleShocked;

                case PlayerExpression.Confused:
                    return manager.maleConfused;

                default:
                    return manager.maleNeutral;
            }
        }

        switch (expression)
        {
            case PlayerExpression.Happy:
                return manager.femaleHappy;

            case PlayerExpression.Angry:
                return manager.femaleAngry;

            case PlayerExpression.Shocked:
                return manager.femaleShocked;

            case PlayerExpression.Confused:
                return manager.femaleConfused;

            default:
                return manager.femaleNeutral;
        }
    }
    public static void ShowLine(
        DialogueLine line,
        TMP_Text speakerText,
        TMP_Text dialogueText,
        PortraitSlot leftPortrait,
        PortraitSlot rightPortrait
    )
    {
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

        Sprite portrait = null;

        if (line.usePlayerPortrait)
        {
            portrait =
                GetPlayerPortrait(
                    line.playerExpression
                );
        }
        else
        {
            portrait =
                line.portrait;
        }

        leftPortrait.gameObject.SetActive(false);
        rightPortrait.gameObject.SetActive(false);

        if (portrait != null)
        {
            if (line.portraitSide ==
                PortraitSide.Left)
            {
                leftPortrait.SetPortrait(
                    portrait
                );
            }
            else
            {
                rightPortrait.SetPortrait(
                    portrait
                );
            }
        }
    }
}