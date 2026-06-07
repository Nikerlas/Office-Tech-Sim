using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<GameObject> inventoryItems = new List<GameObject>();

    void Awake() { Instance = this; }

    public void AddToInventory(GameObject partObject)
    {
        partObject.SetActive(false); // Sembunyikan dari scene
        inventoryItems.Add(partObject);
        Debug.Log("Item masuk inventory: " + partObject.name);
    }
}