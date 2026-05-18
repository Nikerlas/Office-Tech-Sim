using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButton : MonoBehaviour
{
    public GameObject partPrefab;
    public PartType partType;

    public void SelectPart()
    {
        BuildManager.Instance.SelectPart(partPrefab, partType);
    }
}
