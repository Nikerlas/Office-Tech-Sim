using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildTask
{
    public string taskName;

    public List<PartType> requiredParts;
}