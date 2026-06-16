using UnityEngine;

[CreateAssetMenu(menuName = "PC/Part Data")]
public class PartData : ScriptableObject
{
    [Header("Info")]
    public string partName;

    public PartType partType;

    [Header("PC State Value")]
    public string cpuName;

    public int ramSize;

    public string gpuName;

    [Header("Prefab")]
    public GameObject prefab;
}