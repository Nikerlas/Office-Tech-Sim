using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Chapter Data")]
public class ChapterData : ScriptableObject
{
    public string chapterName;
    public int targetMoney;

    [Header("Customer Pool")]
    public List<CustomerJob> customerPool;

    [Header("Day Dialogues")]
    public List<DialogueData> startDayDialogues;
    public List<DialogueData> endDayDialogues;

    [Header("Story Events")]
    public List<StoryEventData> storyEvents;
}