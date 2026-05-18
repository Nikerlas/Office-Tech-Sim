using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;
    public LayerMask placementLayer;

    public GameObject currentPreview;
    public PartType currentPartType;

    public GameObject InventoryUI;
    float holdTimer = 0f;

    public float holdDuration = 1f;

    PartSlot hoveredSlot;
    public List<PartSlot> allSlots = new List<PartSlot>();

    void Awake()
    {
        Instance = this;
    }

    public void SelectPart(GameObject prefab, PartType type)
    {
        currentPartType = type;

        InventoryUI.SetActive(false);

        if (currentPreview != null)
            Destroy(currentPreview);

        currentPreview = Instantiate(prefab);

        Collider col = currentPreview.GetComponent<Collider>();

        if (col != null)
            col.enabled = false;
    }

    void Update()
    {
        CheckRemovePart();
        if (currentPreview == null)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, placementLayer))
        {
            currentPreview.transform.position = hit.point;

            PartSlot slot = hit.collider.GetComponent<PartSlot>();

            if (slot != null)
            {
                Debug.Log("Slot Detected");
                if (slot.allowedType == currentPartType && !slot.occupied)
                {
                    currentPreview.transform.position = slot.transform.position;
                    currentPreview.transform.rotation = slot.transform.rotation;

                    if (Input.GetMouseButtonDown(0))
                    {
                        PlacePart(slot);
                    }
                }
            }
        }

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);

    }

    void PlacePart(PartSlot slot)
    {
        slot.occupied = true;

        TaskManager.Instance.RegisterInstalledPart(currentPartType);

        slot.placedPart = currentPreview;

        GameObject placedPart = currentPreview;

        placedPart.transform.position = slot.snapPoint.position;
        placedPart.transform.rotation = slot.snapPoint.rotation;

        currentPreview = null;

        InventoryUI.SetActive(true);
    }

    void CheckRemovePart()
    {
        if(currentPreview != null)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            PartSlot slot = hit.collider.GetComponent<PartSlot>();

            if(slot != null && slot.occupied)
            {
                hoveredSlot = slot;

                if(Input.GetMouseButton(0))
                {
                    holdTimer += Time.deltaTime;

                    if(holdTimer >= holdDuration)
                    {
                        RemovePart(slot);

                        holdTimer = 0f;
                    }
                }

                if(Input.GetMouseButtonUp(0))
                {
                    holdTimer = 0f;
                }
            }
            else
            {
                holdTimer = 0f;
            }
        }
    }

    void RemovePart(PartSlot slot)
    {
        PartType removedType = slot.allowedType;

        TaskManager.Instance.RemoveInstalledPart(removedType);
        
        Destroy(slot.placedPart);

        slot.placedPart = null;

        slot.occupied = false;
    }

    public void ClearAllParts()
    {
        foreach(PartSlot slot in allSlots)
        {
            if(slot.occupied)
            {
                Destroy(slot.placedPart);

                slot.placedPart = null;

                slot.occupied = false;
            }
        }
    }
}
