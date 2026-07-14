using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager Instance;
    public string playerName;
    public PlayerGender playerGender;
    public bool hasCreatedCharacter;

    public CharacterData playerMale;
    public CharacterData playerFemale;

    [Header("Customer")]
    public List<CustomerData> todayCustomers = new List<CustomerData>();
    public CustomerJob forcedCustomerToday;

    [Header("Customer Progress")]
    public List<CustomerProgress> customerProgress = new List<CustomerProgress>();

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
    public bool playingChapterIntro;
    public bool playingChapterComplete;
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

        if(currentChapterIndex >= chapters.Count)
{
            SceneTransitionManager.Instance
                .FadeToScene("Credit");

            return;
}    
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

        Debug.Log(
            "Money: "
            + currentMoney
            + "/"
            + currentChapter.targetMoney
        );

        Debug.Log(
            "Story Complete: "
            + IsAllStoryCompleted()
        );

        if (currentMoney >= currentChapter.targetMoney && IsAllStoryCompleted())
        {
            chapterComplete = true;

            Debug.Log(
                "CHAPTER COMPLETE"
            );
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

    public CustomerProgress GetCustomerProgress(CustomerData customer)
    {
        foreach (
            CustomerProgress progress
            in customerProgress
        )
        {
            if (progress.customer == customer)
            {
                return progress;
            }
        }

        CustomerProgress newProgress =
            new CustomerProgress();

        newProgress.currentPC =
            customer.startingPC.Clone();

        newProgress.customer =
            customer;

        newProgress.currentStage = 0;

        newProgress.isCompleted = false;

        customerProgress.Add(
            newProgress
        );

        return newProgress;
    }

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

        List<CustomerData> availableCustomers = new List<CustomerData>();

        foreach (
            CustomerData customer
            in currentChapter.customerPool
        )
        {
            if (customer.isStoryCustomer)
            {
                CustomerProgress progress =
                    GetCustomerProgress(customer);

                if (!progress.isCompleted)
                {
                    availableCustomers.Add(customer);
                }
            }
            else
            {
                availableCustomers.Add(customer);
            }
        }

        if (availableCustomers.Count == 0)
        {
            Debug.Log(
                "All Story Customers Completed"
            );

            return;
        }

        int customerCount =
            Mathf.Min(
                3,
                availableCustomers.Count
            );

        for (int i = 0; i < customerCount; i++)
        {
            int randomIndex =
                Random.Range(
                    0,
                    availableCustomers.Count
                );

            CustomerData selected =
                availableCustomers[randomIndex];

            todayCustomers.Add(
                selected
            );

            availableCustomers.RemoveAt(
                randomIndex
            );
        }
    }

    public void SetForcedCustomer(CustomerJob customer)
    {
        forcedCustomerToday = customer;
    }

    public CustomerData GetCurrentTodayCustomer()
    {
        return todayCustomers[currentCustomerIndex];
    }

    public bool IsAllStoryCompleted()
    {
        foreach (
            CustomerData customer
            in currentChapter.customerPool
        )
        {
            if (!customer.isStoryCustomer)
            {
                continue;
            }

            CustomerProgress progress =
                GetCustomerProgress(customer);

            if (!progress.isCompleted)
            {
                return false;
            }
        }

        return true;
    }
    #endregion

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        Debug.Log("Uang berhasil ditambahkan di GameManager: " + currentMoney);

        // Cek chapter unlock jika diperlukan
        // ChapterManager.Instance.CheckChapterUnlock(); 
    }
}