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
        EconomyManager.Instance.AddMoney(
            currentTask.rewardMoney
        );

        BuildManager.Instance.ClearAllParts();

        installedParts.Clear();

        GameManager.Instance.returningFromAssembly = true;

        SceneManager.LoadScene("DialogueScene");
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
                installed ? "☑ " : "☐ ";

            display +=
                check + requiredPart + "\n";
        }

        taskText.text = display;
    }
}