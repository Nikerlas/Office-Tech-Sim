using UnityEngine;

public class ChapterManager : MonoBehaviour
{
    public static ChapterManager Instance;

    public int currentChapter = 1;

    public int chapter2Requirement = 1000;

    void Awake()
    {
        Instance = this;
    }

    public void CheckChapterUnlock()
    {
        if(currentChapter == 1)
        {
            if(GameManager.Instance.currentMoney >= chapter2Requirement)
            {
                UnlockChapter2();
            }
        }
    }

    void UnlockChapter2()
    {
        currentChapter = 2;

        Debug.Log("CHAPTER 2 UNLOCKED");
    }
}