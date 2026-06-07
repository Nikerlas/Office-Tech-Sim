using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;
    public enum BuildMode { Pasang, Lepas }
    public BuildMode currentMode = BuildMode.Pasang;

    public LayerMask placementLayer;
    public GameObject currentPreview;
    public PartType currentPartType;
    public GameObject InventoryUI;
    
    [Header("Remove Settings")]
    public float holdDuration = 2f;
    float holdTimer = 0f;

    void Awake() { Instance = this; }

    // GANTI bagian SetMode(int modeIndex) dengan dua fungsi ini:

    public void SetModePasang() 
    {
        currentMode = BuildMode.Pasang;
        Debug.Log("Mode Pasang Aktif");
    }

    public void SetModeLepas() 
    {
        currentMode = BuildMode.Lepas;
        Debug.Log("Mode Lepas Aktif");
    }

    public void SelectPart(GameObject prefab, PartType type)
    {
        if (currentMode != BuildMode.Pasang) return; // Hanya bisa pasang di mode Pasang
        
        currentPartType = type;
        InventoryUI.SetActive(false);
        if (currentPreview != null) Destroy(currentPreview);
        currentPreview = Instantiate(prefab);
        currentPreview.GetComponent<Collider>().enabled = false;
    }

    void Update()
    {
        if (currentMode == BuildMode.Pasang)
        {
            HandlePlacement();
        }
        else if (currentMode == BuildMode.Lepas)
        {
            CheckRemovePart();
        }
    }

    void HandlePlacement()
    {
        if (currentPreview == null) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, placementLayer))
        {
            currentPreview.transform.position = hit.point;
            PartSlot slot = hit.collider.GetComponent<PartSlot>();
            if (slot != null && slot.allowedType == currentPartType && !slot.occupied)
            {
                currentPreview.transform.position = slot.transform.position;
                if (Input.GetMouseButtonDown(0)) PlacePart(slot);
            }
        }
    }

    void CheckRemovePart()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            PartSlot slot = hit.collider.GetComponent<PartSlot>();
            if (slot != null && slot.occupied)
            {
                if (Input.GetMouseButton(0))
                {
                    holdTimer += Time.deltaTime;
                    if (holdTimer >= holdDuration)
                    {
                        RemovePartToInventory(slot);
                        holdTimer = 0f;
                    }
                }
                else { holdTimer = 0f; }
            }
            else { holdTimer = 0f; }
        }
    }

    void RemovePartToInventory(PartSlot slot)
    {
        // Masukkan ke InventoryManager
        InventoryManager.Instance.AddToInventory(slot.placedPart);
        
        // Update TaskManager
        TaskManager.Instance.RemoveInstalledPart(slot.placedPart.GetComponent<PCPart>().partType);
        
        // Bersihkan slot
        slot.placedPart = null;
        slot.occupied = false;
        Debug.Log("Part berhasil dicopot!");
    }

    void PlacePart(PartSlot slot)
    {
        slot.occupied = true;
        slot.placedPart = currentPreview;
        slot.placedPart.transform.position = slot.snapPoint.position;
        slot.placedPart.transform.rotation = slot.snapPoint.rotation;
        TaskManager.Instance.RegisterInstalledPart(currentPartType);
        currentPreview = null;
        InventoryUI.SetActive(true);
    }
}