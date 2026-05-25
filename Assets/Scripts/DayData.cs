using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Day Data")]
public class DayData : ScriptableObject
{
    public List<CustomerJob> customerJobs;
}