using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager Instance;
    public string playerName;

    [Header("Customer")]
    public List<CustomerJob> todayCustomers =
    new List<CustomerJob>();
    public CustomerJob forcedCustomerToday;

    [Header("Progression")]
    public int currentChapterIndex;
    public int currentDayIndex;
    public int currentCustomerIndex;

    [Header("Economy")]
    public int currentMoney;

    [Header("Chapters")]
    public List<ChapterData> chapters = new List<ChapterData>();

    [Header("Runtime Data")]
    public ChapterData currentChapter;

    public BuildTask currentTask;

    public bool returningFromAssembly;
    public bool dayFinished;
    public bool chapterComplete;
    #endregion

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Chapter & Day Progression
    public void LoadCurrentChapter()
    {
        currentChapter =
            chapters[currentChapterIndex];
    }

    void DayComplete()
    {
        Debug.Log("DAY COMPLETE");

        dayFinished = true;

        currentCustomerIndex = 0;
    }

    public void SleepAndProgress()
    {
        dayFinished = false;

        if (currentMoney >= currentChapter.targetMoney)
        {
            chapterComplete = true;

            Debug.Log("CHAPTER COMPLETE");
        }
        else
        {
            currentDayIndex++;

            currentCustomerIndex = 0;

            Debug.Log("NEXT DAY");

            GenerateTodayCustomers();
        }
    }

    public StoryEventData GetTodayStoryEvent()
    {
        foreach (StoryEventData storyEvent
            in currentChapter.storyEvents)
        {
            if (storyEvent.triggerDay ==
                currentDayIndex + 1)
            {
                return storyEvent;
            }
        }

        return null;
    }
    #endregion

    #region Customer
    public void NextCustomer()
    {
        currentCustomerIndex++;

        if (currentCustomerIndex >= 3)
        {
            DayComplete();

            return;
        }
    }

    public void GenerateTodayCustomers()
    {
        todayCustomers.Clear();

        List<CustomerJob> pool =
            currentChapter.customerPool;

        for (int i = 0; i < 3; i++)
        {
            int randomIndex =
                Random.Range(0, pool.Count);

            todayCustomers.Add(
                pool[randomIndex]
            );
        }

        if (forcedCustomerToday != null)
        {
            int slot =
                Random.Range(0, todayCustomers.Count);

            todayCustomers[slot] =
                forcedCustomerToday;

            forcedCustomerToday = null;
        }

        Debug.Log("Today's Customers:");

        foreach (CustomerJob customer
            in todayCustomers)
        {
            Debug.Log(customer.name);
        }
    }

    public void SetForcedCustomer(CustomerJob customer)
    {
        forcedCustomerToday = customer;
    }

    public CustomerJob GetCurrentTodayCustomer()
    {
        return todayCustomers[currentCustomerIndex];
    }
    #endregion
}