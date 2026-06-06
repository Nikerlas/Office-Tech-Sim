using UnityEngine;

[CreateAssetMenu(menuName = "Game/Customer Job")]
public class CustomerJob : ScriptableObject
{
    public string customerName;

    public DialogueData introDialogue;

    public BuildTask buildTask;

    public DialogueData resultDialogue;
}