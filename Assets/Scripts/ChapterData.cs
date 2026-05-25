using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Chapter Data")]
public class ChapterData : ScriptableObject
{
    public string chapterName;

    public int targetMoney;

    public List<DayData> days;
}