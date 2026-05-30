using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Day Data")]
public class DayData : ScriptableObject
{
    [Header("Day Story")]
    public DialogueData startDayDialogue;

    [Header("Customers")]
    public List<CustomerJob> customerJobs;

    [Header("Day Story")]
    public DialogueData endDayDialogue;
}