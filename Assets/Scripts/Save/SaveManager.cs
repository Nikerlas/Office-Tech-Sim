using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame()
    {
        SaveData data =
            new SaveData();

        data.playerName =
            GameManager.Instance.playerName;

        data.currentMoney =
            GameManager.Instance.currentMoney;

        data.currentChapterIndex =
            GameManager.Instance.currentChapterIndex;

        data.currentDayIndex =
            GameManager.Instance.currentDayIndex;

        string json =
            JsonUtility.ToJson(data);

        PlayerPrefs.SetString(
            "SaveData",
            json
        );

        PlayerPrefs.Save();

        Debug.Log("GAME SAVED");
    }

    public void LoadGame()
    {
        if(!PlayerPrefs.HasKey("SaveData"))
        {
            Debug.Log("NO SAVE FOUND");

            return;
        }

        string json =
            PlayerPrefs.GetString("SaveData");

        SaveData data =
            JsonUtility.FromJson<SaveData>(json);

        GameManager.Instance.playerName =
            data.playerName;

        GameManager.Instance.currentMoney =
            data.currentMoney;

        GameManager.Instance.currentChapterIndex =
            data.currentChapterIndex;

        GameManager.Instance.currentDayIndex =
            data.currentDayIndex;

        GameManager.Instance.LoadCurrentChapter();

        GameManager.Instance.GenerateTodayCustomers();

        Debug.Log("SAVE LOADED");
    }
}