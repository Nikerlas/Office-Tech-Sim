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

    void Start()
    {
        LoadCustomerPC();
    }

    void Update()
    {
        // --- FUNGSI UNDO/CANCEL MENGGUNAKAN ESC ---
        // Jika pemain sedang memegang part (currentPreview tidak kosong) dan menekan tombol ESC
        if (currentPreview != null && Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(currentPreview);     // Hancurkan part preview yang sedang melayang di kursor
            currentPreview = null;       // Reset variable agar statusnya kembali kosong
            InventoryUI.SetActive(true); // Munculkan kembali UI Inventory agar bisa pilih part lain

            Debug.Log("Pemilihan part dibatalkan via ESC (Undo)");
            return;                      // Stop baris kode di bawahnya agar tidak menjalankan raycast di frame ini
        }
        // ------------------------------------------

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

    public void ToggleInventory()
    {
        // Cek apakah inventory sedang aktif atau tidak, lalu balik kondisinya
        bool isActive = InventoryUI.activeSelf;
        InventoryUI.SetActive(!isActive);
    }

    public void LoadCustomerPC()
    {
        CustomerData customer =
            GameManager.Instance
                .GetCurrentTodayCustomer();

        CustomerProgress progress =
            GameManager.Instance
                .GetCustomerProgress(customer);

        CustomerPC pc =
            progress.currentPC;

        SpawnCPU(pc.cpu);
        SpawnRAM(pc.ramSize);
        SpawnGPU(pc.gpu);
    }

    void PlacePart(PartSlot slot)
    {
        slot.occupied = true;

        TaskManager.Instance.RegisterInstalledPart(slot.allowedType);

        slot.placedPart = currentPreview;

        GameObject placedPart = currentPreview;

        placedPart.transform.position = slot.snapPoint.position;
        placedPart.transform.rotation = slot.snapPoint.rotation;

        currentPreview = null;

        InventoryUI.SetActive(false);
    }

    void CheckRemovePart()
    {
        if (currentPreview != null)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            PartSlot slot = hit.collider.GetComponent<PartSlot>();

            if (slot != null && slot.occupied)
            {
                hoveredSlot = slot;

                if (Input.GetMouseButton(0))
                {
                    holdTimer += Time.deltaTime;

                    if (holdTimer >= holdDuration)
                    {
                        RemovePart(slot);

                        holdTimer = 0f;
                    }
                }

                if (Input.GetMouseButtonUp(0))
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
        foreach (PartSlot slot in allSlots)
        {
            if (slot.occupied)
            {
                Destroy(slot.placedPart);

                slot.placedPart = null;

                slot.occupied = false;
            }
        }
    }

    void SpawnCPU(string cpuName)
    {
        if (string.IsNullOrEmpty(cpuName))
            return;

        PartData cpuData =
            PartDatabase.Instance
                .GetCPU(cpuName);

        if (cpuData == null)
            return;

        SpawnPartToSlot(
            PartType.CPU,
            cpuData.prefab
        );
    }

    void SpawnRAM(int ramSize)
    {
        PartData ramData =
            PartDatabase.Instance
                .GetRAM(ramSize);

        if (ramData == null)
            return;

        SpawnPartToSlot(
            PartType.RAM,
            ramData.prefab
        );
    }

    void SpawnGPU(string gpuName)
    {
        if (string.IsNullOrEmpty(gpuName))
            return;

        PartData gpuData =
            PartDatabase.Instance
                .GetGPU(gpuName);

        if (gpuData == null)
            return;

        SpawnPartToSlot(
            PartType.GPU,
            gpuData.prefab
        );
    }

    void SpawnPartToSlot(PartType type, GameObject prefab)
    {
        foreach (
            PartSlot slot
            in allSlots
        )
        {
            if (
                slot.allowedType == type
                &&
                !slot.occupied
            )
            {
                GameObject part =
                    Instantiate(
                        prefab,
                        slot.snapPoint.position,
                        slot.snapPoint.rotation
                    );

                slot.placedPart = part;

                slot.occupied = true;

                break;
            }
        }
    }

    public PartData GetInstalledPartData(PartType type)
    {
        foreach (
            PartSlot slot
            in allSlots
        )
        {
            if (
                slot.allowedType == type
                &&
                slot.occupied
            )
            {
                PCPart pcPart =
                    slot.placedPart
                        .GetComponent<PCPart>();

                if (pcPart != null)
                {
                    return pcPart.partData;
                }
            }
        }

        return null;
    }
}