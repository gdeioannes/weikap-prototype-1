using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameProgress : MonoBehaviour
{
    public enum LevelStatus
    {
        OnGoing,
        Win,
        Lose
    }

    public static GameProgress Instance { get; private set; }

    [System.Serializable]
    public class LevelData
    {
        public int levelId;
        public LevelStatus status;
        public int maxCoinsCollected;
        public int maxSamplesCollected;
        public int maxRightAnsweredQuestions;    
    }

    [System.Serializable]
    public class QuestionData
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
    public Dictionary<int, LevelData> LevelsData { get; private set; }

    public System.Action OnSamplesCollectionUpdated = delegate { };
    public System.Action<long> OnCoinsAmountUpdated = delegate { };

    void OnAwake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public static void Instantiate()
    {
        GameObject newObject = new GameObject("GamerProgress");
        Instance = newObject.AddComponent<GameProgress>();
        Instance.GetValuesFromPlayerPrefs();
    }

    void GetValuesFromPlayerPrefs()
    {
        string jsonGameData = PlayerPrefs.GetString("gamerProgress");
        gameData = !string.IsNullOrEmpty(jsonGameData) ? JsonUtility.FromJson<GameData>(jsonGameData) : new GameData();

        SamplesCollected = new HashSet<int>();
        ToolsUnlocked = new HashSet<int>();
        LevelsData = new Dictionary<int, LevelData>();

        if (gameData.samplesCollected != null)
        {
            foreach (var item in gameData.samplesCollected)
            {
                SamplesCollected.Add(item);
            }
        }

        if (gameData.toolsUnlocked != null)
        {
            foreach (var item in gameData.toolsUnlocked)
            {
                ToolsUnlocked.Add(item);
            }
        }

        if (gameData.levelsData != null)
        {
            foreach (var item in gameData.levelsData)
            {
                LevelsData[item.levelId] = item;
            }
        }
    }

    void SaveValuesToPlayerPrefs()
    {
        if (SamplesCollected.Count > 0)
        {
            gameData.samplesCollected = new int[SamplesCollected.Count];
            var samplesEnumerator = SamplesCollected.GetEnumerator();
            int index = 0;
            while (samplesEnumerator.MoveNext())
            {
                gameData.samplesCollected[index] = samplesEnumerator.Current;
                index++;
            }
        }

        if (ToolsUnlocked.Count > 0)
        {
            gameData.toolsUnlocked = new int[ToolsUnlocked.Count];
            var toolsEnumerator = ToolsUnlocked.GetEnumerator();
            int index = 0;
            while (toolsEnumerator.MoveNext())
            {
                gameData.toolsUnlocked[index] = toolsEnumerator.Current;
                index++;
            }
        }

        if (LevelsData.Count > 0)
        {
            gameData.levelsData = new LevelData[LevelsData.Count];
            var levelsEnumerator = LevelsData.GetEnumerator();
            int index = 0;
            while (levelsEnumerator.MoveNext())
            {
                gameData.levelsData[index] = levelsEnumerator.Current.Value;
            }
        }

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

    public void UpdateLevelProgress(int levelId, Dictionary<InGameItemsDBScriptableObject.ItemType, int> consumablesCollected, GameProgress.LevelStatus status)
    {
        if (!LevelsData.ContainsKey(levelId))
        {
            LevelsData[levelId] = new LevelData() { levelId = levelId };            
        }
        LevelsData[levelId].maxCoinsCollected = Mathf.Max(LevelsData[levelId].maxCoinsCollected, consumablesCollected.TryGetValue(InGameItemsDBScriptableObject.ItemType.Coin,0));
        LevelsData[levelId].maxSamplesCollected = Mathf.Max(LevelsData[levelId].maxSamplesCollected, consumablesCollected.TryGetValue(InGameItemsDBScriptableObject.ItemType.Sample, 0));
        LevelsData[levelId].maxRightAnsweredQuestions = Mathf.Max(LevelsData[levelId].maxRightAnsweredQuestions, consumablesCollected.TryGetValue(InGameItemsDBScriptableObject.ItemType.Question, 0));

        if (LevelsData[levelId].status == LevelStatus.OnGoing || (LevelsData[levelId].status == LevelStatus.Lose && status == LevelStatus.Win))
        {
            LevelsData[levelId].status = status;
        }        
    }

    void OnDestroy()
    {
        SaveValuesToPlayerPrefs();
        Instance = null;
    }
}
