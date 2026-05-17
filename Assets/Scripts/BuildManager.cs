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

        GameObject placedPart = currentPreview;

        placedPart.transform.position = slot.snapPoint.position;
        placedPart.transform.rotation = slot.snapPoint.rotation;

        currentPreview = null;

        InventoryUI.SetActive(true);
    }
}
