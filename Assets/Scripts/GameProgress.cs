using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameProgress : MonoBehaviour
{
    public static GameProgress Instance { get; private set; }

    [System.Serializable]
    public struct LevelData
    {
        public int levelId;        
        public int maxCoinsCollected;
        public int maxSamplesCollected;
        public int maxRightAnsweredQuestions;
    }

    [System.Serializable]
    public struct QuestionData
    {
        public int questionId;
        public int rightAnswers;
        public int wrongAnsers;
    }

    [System.Serializable]
    public struct GameData
    {
        public long coinsAvailable;
        public int[] samplesCollected;
        public int[] toolsUnlocked;

        public LevelData[] levelsData;

        public ulong totalGameTime;
        public int[] finishedStages;
        public int[] lostStages;

        public QuestionData[] questionsData;
        public long coinsCollected;
    }

    GameData gameData;

    public long CoinsCollected { get { return gameData.coinsCollected; } }
    public long CoinsAvailable { get { return gameData.coinsAvailable; } }
    public HashSet<int> SamplesCollected { get; private set; }
    public HashSet<int> ToolsUnlocked { get; private set; }

    public System.Action OnSamplesCollectionUpdated = delegate { };
    public System.Action<long> OnCoinsAmountUpdated = delegate { };

    void OnAwake()
    {
        Object.DontDestroyOnLoad(this.gameObject);
    }

    public static void Instantiate()
    {
        GameObject newObject = new GameObject("GamerProgress");
        Instance = newObject.AddComponent<GameProgress>();
        Instance.SetValuesFromPlayerPrefs();
    }

    void SetValuesFromPlayerPrefs()
    {
        string jsonGameData = PlayerPrefs.GetString("gamerProgress");
        gameData = !string.IsNullOrEmpty(jsonGameData) ? JsonUtility.FromJson<GameData>(jsonGameData) : new GameData();

        SamplesCollected = new HashSet<int>();
        ToolsUnlocked = new HashSet<int>();        
    }

    void SaveValuesToPlayerPrefs()
    {
        string jsonGameData = JsonUtility.ToJson(gameData);
        PlayerPrefs.SetString("gamerProgress", jsonGameData);
    }

    public void UpdateSamplesCollected(int newSampleId)
    {
        SamplesCollected.Add(newSampleId);
        OnSamplesCollectionUpdated();
    }

    public void UpdateCoinsCollected(long coins)
    {
        gameData.coinsAvailable += coins;
        gameData.coinsCollected = coins > 0 ? gameData.coinsCollected + coins : gameData.coinsCollected;
        OnCoinsAmountUpdated(gameData.coinsAvailable);
    }

    void OnDestroy()
    {
        SaveValuesToPlayerPrefs();
        Instance = null;
    }
}
