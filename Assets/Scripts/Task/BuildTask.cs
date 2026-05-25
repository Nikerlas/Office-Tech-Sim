using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Mission/Build Task")]
public class BuildTask : ScriptableObject
{
    public string taskName;

    public List<PartType> requiredParts;

    public int rewardMoney;
}