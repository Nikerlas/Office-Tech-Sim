using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public Sprite portrait;

    public bool usePlayerPortrait;

    public PlayerExpression playerExpression;

    public string speakerName;

    [TextArea(3,5)]
    public string dialogueText;
}