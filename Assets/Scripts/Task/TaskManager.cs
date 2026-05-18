using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    public TMP_Text taskText;

    public List<BuildTask> tasks;

    int currentTaskIndex = 0;

    List<PartType> installedParts = new List<PartType>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateTaskUI();
    }

    public void RegisterInstalledPart(PartType part)
    {
        if(installedParts.Contains(part))
            return;

        installedParts.Add(part);

        Debug.Log(part + " Installed");
    }

    public void RemoveInstalledPart(PartType part)
    {
        if(installedParts.Contains(part))
        {
            installedParts.Remove(part);

            Debug.Log(part + " Removed");
        }
    }

    public void CheckCurrentTask()
    {
        if(currentTaskIndex >= tasks.Count)
             return;

        BuildTask currentTask = tasks[currentTaskIndex];

        foreach(PartType requiredPart in currentTask.requiredParts)
        {
            if(!installedParts.Contains(requiredPart))
            {
                Debug.Log("Task Belum Lengkap");
                return;
            }
        }

        TaskComplete();
    }

    void TaskComplete()
    {
        Debug.Log("TASK CLEAR");

        BuildManager.Instance.ClearAllParts();

        currentTaskIndex++;

        installedParts.Clear();

        if(currentTaskIndex >= tasks.Count)
        {
            taskText.text = "ALL TASK COMPLETE";

            Debug.Log("SEMUA TASK SELESAI");

            return;
        }

        UpdateTaskUI();
    }

    void UpdateTaskUI()
    {
        if(currentTaskIndex >= tasks.Count)
        {
            taskText.text = "ALL TASK COMPLETE";
            return;
        }

        BuildTask currentTask = tasks[currentTaskIndex];

        taskText.text = currentTask.taskName;
    }
}