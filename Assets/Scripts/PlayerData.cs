using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[Prefab()]
public class PlayerData : Singleton<PlayerData>
{
    public enum LevelStatus
    {
        OnGoing,
        Win,
        Lose
    }

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
    public struct GameData
    {
        public long coinsAvailable;
        public int[] samplesCollected;
        public int[] toolsUnlocked;

        public LevelData[] levelsData;

        public long totalGameTime;

        public long coinsCollected;
    }

    GameData gameData;

	[SerializeField] LevelsDBScriptableObject levelsDB;

    public long CoinsCollected { get { return gameData.coinsCollected; } }
    public long CoinsAvailable { get { return gameData.coinsAvailable; } }
	public long TotalGameTime { get { return gameData.totalGameTime; } }

    public HashSet<int> SamplesCollected { get; private set; }
    public HashSet<int> ToolsUnlocked { get; private set; }
    public Dictionary<int, LevelData> LevelsData { get; private set; }
	public Dictionary<int, LevelsDBScriptableObject.Level> Levels { get; private set; }

    public System.Action OnSamplesCollectionUpdated = delegate { };
	public System.Action OnToolsUnlockUpdated = delegate { };
	public System.Action<long> OnCoinsAmountUpdated = delegate { };

	void Awake()
	{
		this.GetValuesFromPlayerPrefs ();

		Levels = new Dictionary<int, LevelsDBScriptableObject.Level> ();
		for (int i = 0, max = levelsDB.levels.Length; i < max; ++i) 
		{
			Levels [i] = levelsDB.levels [i];
		}

		StartCoroutine (UpdateGameTime ());
	}

	IEnumerator UpdateGameTime()
	{
		var yieldInstruction = new WaitForSeconds (1);

		while (true) 
		{
			gameData.totalGameTime += 1;
			yield return yieldInstruction;
		}
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
		if (SamplesCollected != null && SamplesCollected.Count > 0)
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

		if ( ToolsUnlocked != null && ToolsUnlocked.Count > 0)
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

		if (LevelsData != null && LevelsData.Count > 0)
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

	public bool BuyTool(int toolId)
	{
		if (ToolsUnlocked.Contains(toolId)) { return false; } // do nothing, already purchased
		if (toolId < 0 || toolId >= GameController.Instance.ToolsDB.Length){ return false; } // invalid tool id
		var toolData = GameController.Instance.ToolsDB[toolId];
		if(gameData.coinsAvailable < toolData.unlockCost)
		{
			return false; // unable to buy, not enough coins
		}

		UpdateCoinsCollected(-toolData.unlockCost);
		ToolsUnlocked.Add(toolId);
		OnToolsUnlockUpdated();
		return true;
	}

    public void UpdateCoinsCollected(long coins)
    {
        gameData.coinsAvailable += coins;
        gameData.coinsCollected = coins > 0 ? gameData.coinsCollected + coins : gameData.coinsCollected;
        OnCoinsAmountUpdated(gameData.coinsAvailable);
    }

	public void UpdateLevelProgress(int levelId, Dictionary<InGameItemsDBScriptableObject.ItemType, int> consumablesCollected, PlayerData.LevelStatus status)
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
    }
}
