using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public Sprite portrait;
    public string speakerName;

    [TextArea(3,5)]
    public string dialogueText;
}