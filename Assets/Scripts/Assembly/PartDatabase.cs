using System.Collections.Generic;
using UnityEngine;

public class PartDatabase : MonoBehaviour
{
    public static PartDatabase Instance;

    public List<PartData> allParts =
        new List<PartData>();

    void Awake()
    {
        Instance = this;
    }

    public PartData GetCPU(
        string cpuName
    )
    {
        foreach (
            PartData part
            in allParts
        )
        {
            if (
                part.partType == PartType.CPU
                &&
                part.cpuName == cpuName
            )
            {
                return part;
            }
        }

        return null;
    }

    public PartData GetRAM(
        int ramSize
    )
    {
        foreach (
            PartData part
            in allParts
        )
        {
            if (
                part.partType == PartType.RAM
                &&
                part.ramSize == ramSize
            )
            {
                return part;
            }
        }

        return null;
    }

    public PartData GetGPU(
        string gpuName
    )
    {
        foreach (
            PartData part
            in allParts
        )
        {
            if (
                part.partType == PartType.GPU
                &&
                part.gpuName == gpuName
            )
            {
                return part;
            }
        }

        return null;
    }
}