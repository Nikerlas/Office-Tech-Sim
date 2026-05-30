using UnityEngine;

[CreateAssetMenu(menuName = "Game/Story Event")]
public class StoryEventData : ScriptableObject
{
    public int triggerDay;

    public DialogueData eventDialogue;

    public CustomerJob forcedCustomer;
}