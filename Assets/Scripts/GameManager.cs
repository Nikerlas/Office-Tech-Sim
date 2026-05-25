using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Progression")]
    public int currentChapterIndex;
    public int currentDayIndex;
    public int currentCustomerIndex;

    [Header("Economy")]
    public int currentMoney;

    [Header("Runtime Data")]
    public ChapterData currentChapter;

    public BuildTask currentTask;

    public bool returningFromAssembly;
    public bool dayFinished;

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

    public CustomerJob GetCurrentCustomerJob()
    {
        return currentChapter
            .days[currentDayIndex]
            .customerJobs[currentCustomerIndex];
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

    void DayComplete()
    {
        Debug.Log("DAY COMPLETE");

        dayFinished = true;

        CheckChapterComplete();
    }

    void CheckChapterComplete()
    {
        if (currentMoney >= currentChapter.targetMoney)
        {
            Debug.Log("CHAPTER COMPLETE");

            currentChapterIndex++;

            currentDayIndex = 0;

            currentCustomerIndex = 0;

            // load next chapter nanti
        }
        else
        {
            Debug.Log("TARGET NOT REACHED");

            currentDayIndex++;

            currentCustomerIndex = 0;

            Debug.Log("CONTINUE NEXT DAY");
        }
    }
}