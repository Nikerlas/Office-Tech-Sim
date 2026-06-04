using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class DialoguePresenter
{
    static Sprite GetPlayerPortrait(
    PlayerExpression expression
)
    {
        CharacterData playerData =
            GameManager.Instance.playerGender ==
            PlayerGender.Male
            ? GameManager.Instance.playerMale
            : GameManager.Instance.playerFemale;

        switch (expression)
        {
            case PlayerExpression.Happy:
                return playerData.happy;

            case PlayerExpression.Angry:
                return playerData.angry;

            case PlayerExpression.Shocked:
                return playerData.shocked;

            case PlayerExpression.Confused:
                return playerData.confused;

            default:
                return playerData.neutral;
        }
    }

    public static string BuildDialogueText(
    DialogueLine line
)
    {
        return line.dialogueText.Replace(
            "{PLAYER}",
            GameManager.Instance.playerName
        );
    }

    public static void ShowLine(
        DialogueLine line,
        TMP_Text speakerText,
        TMP_Text dialogueText,
        PortraitSlot leftPortrait,
        PortraitSlot rightPortrait,
        PortraitSlot centerPortrait
    )
    {
        speakerText.text =
            line.speakerName.Replace(
                "{PLAYER}",
                GameManager.Instance.playerName
            );

        dialogueText.text = "";

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

        switch (line.portraitSide)
        {
            case PortraitSide.Left:

                leftPortrait.SetPortrait(
                    portrait
                );

                switch (line.portraitAnimation)
                {
                    case PortraitAnimation.Bounce:

                        leftPortrait.PlaySpawnAnimation();

                        break;
                }

                if (leftPortrait.HasPortrait())
                {
                    leftPortrait.Highlight();
                }

                if (centerPortrait.HasPortrait())
                {
                    centerPortrait.Unhighlight();
                }

                if (rightPortrait.HasPortrait())
                {
                    rightPortrait.Unhighlight();
                }

                break;

            case PortraitSide.Center:

                centerPortrait.SetPortrait(
                    portrait
                );

                switch (line.portraitAnimation)
                {
                    case PortraitAnimation.Bounce:

                        centerPortrait.PlaySpawnAnimation();

                        break;
                }

                if (centerPortrait.HasPortrait())
                {
                    centerPortrait.Highlight();
                }

                if (leftPortrait.HasPortrait())
                {
                    leftPortrait.Unhighlight();
                }

                if (rightPortrait.HasPortrait())
                {
                    rightPortrait.Unhighlight();
                }

                break;

            case PortraitSide.Right:

                rightPortrait.SetPortrait(
                    portrait
                );

                switch (line.portraitAnimation)
                {
                    case PortraitAnimation.Bounce:

                        rightPortrait.PlaySpawnAnimation();

                        break;
                }

                if (rightPortrait.HasPortrait())
                {
                    rightPortrait.Highlight();
                }

                if (leftPortrait.HasPortrait())
                {
                    leftPortrait.Unhighlight();
                }

                if (centerPortrait.HasPortrait())
                {
                    centerPortrait.Unhighlight();
                }

                break;
        }
    }
}