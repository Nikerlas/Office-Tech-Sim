using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    BuildTask currentTask;

    public TMP_Text taskText;

    List<PartType> installedParts = new List<PartType>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentTask = GameManager.Instance.currentTask;

        if (currentTask == null)
        {
            Debug.LogError("NO TASK FOUND");

            return;
        }

        UpdateTaskUI();
    }

    public void RegisterInstalledPart(PartType part)
    {
        if (installedParts.Contains(part))
            return;

        installedParts.Add(part);

        Debug.Log(part + " Installed");

        UpdateTaskUI();
    }

    public void RemoveInstalledPart(PartType part)
    {
        if (installedParts.Contains(part))
        {
            installedParts.Remove(part);

            Debug.Log(part + " Removed");

            UpdateTaskUI();
        }
    }

    public void CheckCurrentTask()
    {
        PartData ramData = BuildManager.Instance.GetInstalledPartData(
            PartType.RAM
        );

        if (ramData != null)
        {
            Debug.Log(
                "Installed RAM = "
                + ramData.ramSize
            );
        }
        foreach (PartType requiredPart
            in currentTask.requiredParts)
        {
            if (!installedParts.Contains(requiredPart))
            {
                Debug.Log("Task Belum Lengkap");

                return;
            }
        }

        TaskComplete();
    }

    void TaskComplete()
    {
        // SEBELUMNYA: EconomyManager.Instance.AddMoney(...)
        // UBAH MENJADI:
        GameManager.Instance.AddMoney(currentTask.rewardMoney);

        CustomerData customer = GameManager.Instance.GetCurrentTodayCustomer();

        CustomerProgress progress = GameManager.Instance.GetCustomerProgress(customer);

        if (!string.IsNullOrEmpty(
    currentTask.targetCPU))
        {
            progress.currentPC.cpu =
                currentTask.targetCPU;
        }

        if (currentTask.targetRAM > 0)
        {
            progress.currentPC.ramSize =
                currentTask.targetRAM;
        }

        if (!string.IsNullOrEmpty(
            currentTask.targetGPU))
        {
            progress.currentPC.gpu =
                currentTask.targetGPU;
        }

        Debug.Log(
            customer.customerName +
            " PC Updated\n" +
            "CPU: " + progress.currentPC.cpu +
            "\nRAM: " + progress.currentPC.ramSize +
            "\nGPU: " + progress.currentPC.gpu
        );

        //BuildManager.Instance.ClearAllParts();
        installedParts.Clear();
        GameManager.Instance.returningFromAssembly = true;

        SceneTransitionManager.Instance.FadeToScene("DialogueScene");
    }

    void UpdateTaskUI()
    {
        string display =
            currentTask.taskName + "\n\n";

        foreach (PartType requiredPart
            in currentTask.requiredParts)
        {
            bool installed =
                installedParts.Contains(requiredPart);

            string check =
                installed ? "[X] " : "[  ] ";

            display +=
                check + requiredPart + "\n";
        }

        taskText.text = display;
    }
}