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

        List<CustomerJob> availableCustomers = new List<CustomerJob>(currentChapter.customerPool);

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

            CustomerJob selected =
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

    public CustomerJob GetCurrentTodayCustomer()
    {
        return todayCustomers[currentCustomerIndex];
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