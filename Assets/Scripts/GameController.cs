using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    [Header("Config")]

    public QuestionsDBScriptableObject questionsDB;    
    public LevelProgress levelProgress;
    [SerializeField] SamplesDBScriptableObject samplesDB;
    [SerializeField] ToolsDBScriptableObject toolsDB;
    [SerializeField] TwoDCameraFollower cameraFollower;
    [SerializeField] LevelContainer levelContainer;
    [SerializeField] CharacterControl characterPrefab;
    [SerializeField] float energyDecreaseInterval = 1f;

    [Header("UI Elements")]
    [SerializeField] UnityEngine.UI.Slider energySlider;    
    [SerializeField] UnityEngine.UI.Text coinsCurrent;
    [SerializeField] UnityEngine.UI.Text coinsMax;    
    [SerializeField] UnityEngine.UI.Text questionsCurrent;
    [SerializeField] UnityEngine.UI.Text questionsMax;
    [SerializeField] SamplesInLevelController samplesUIController;
    [SerializeField] SamplesPopUpController samplesPopUpController;
    [SerializeField] QuestionsPopUpController questionController;

    public Transform CoinsIconContainer;
    public Transform QuestionsIconContainer;
    public Camera WorldCamera;
    public RectTransform UIMainRectTransform;

    public float consumablesMoveToUITime = 2f;
    public DG.Tweening.Ease consumableMovEaseType = DG.Tweening.Ease.InCubic;

    public static GameController Instance { get; private set; }
    public SamplesDBScriptableObject.Sample[] SamplesDB { get { return this.samplesDB.samples; } }

    void Awake()
    {
        Instance = this;
        cameraFollower.enabled = false;
        energySlider.value = 100;
		var gameProgressInstance = GameProgress.Instance; // creating game progress instance
    }

    void Start()
    {
        coinsMax.text = levelContainer.CoinsCount.ToString();
        questionsMax.text = levelContainer.QuestionsCount.ToString();
        samplesUIController.FillSamplesInLevel(levelContainer.samplesInLevel);
        SpawnCharacterOnMap();
    }

    void SpawnCharacterOnMap()
    {
        CharacterControl character = Object.Instantiate<CharacterControl>(characterPrefab);        
        character.OnEnergyUpdated += OnEnergyUpdated;        
        character.StartCoroutine(character.EnergyCountDownCoroutine(energyDecreaseInterval));
        Transform spawnPoint = levelContainer.spawnPoints[Random.Range(0, levelContainer.spawnPoints.Length-1)].transform;
        character.gameObject.transform.SetParent(levelContainer.transform, false);
        character.gameObject.transform.position = spawnPoint.position;
        cameraFollower.target = character.gameObject.transform;
        cameraFollower.enabled = true;
    }

    public void UpdateConsumable(InGameItemsDBScriptableObject.ItemType type, int index, int amount)
    {
        if (levelProgress.consumables.ContainsKey(type))
        {
            levelProgress.consumables[type] += amount;
        }
        else
        {
            levelProgress.consumables[type] = amount;
        }

        if (type == InGameItemsDBScriptableObject.ItemType.Sample)
        {
            GameProgress.Instance.UpdateSamplesCollected(index);
        }
        if (type == InGameItemsDBScriptableObject.ItemType.Coin)
        {
            GameProgress.Instance.UpdateCoinsCollected((long)amount);
        }

        UpdateConsumablesUI(type, levelProgress.consumables[type]);
    }

    void UpdateConsumablesUI(InGameItemsDBScriptableObject.ItemType type, int amount)
    {
        switch (type)
        {
            case InGameItemsDBScriptableObject.ItemType.Coin:
                coinsCurrent.text = amount.ToString();                
                break;
            case InGameItemsDBScriptableObject.ItemType.Question:
                questionsCurrent.text = amount.ToString();
                break;
        }
    }

    void OnEnergyUpdated(float amount)
    {
        energySlider.value = amount;        
    }

    public QuestionsDBScriptableObject.Question GetQuestionInfoById(int questionId)
    {
        return questionsDB.GetQuestionInfoById(questionId);
    }

    public void DisplayQuestion(int questionId, System.Action<bool> onAnswerCb)
    {
        questionController.ShowQuestion(questionId, onAnswerCb);
    }

    public void DisplaySamplesPopUp(int sampleId)
    {
        samplesPopUpController.Show(sampleId);
    }    

    public Transform GetSampleIconContainer(int sampleId)
    {
        return samplesUIController.GetSampleIconTransform(sampleId);
    }

    public void UpdateLevelProgress(GameProgress.LevelStatus status)
    {
        levelProgress.UpdateGameProgress(status);
        switch (status)
        {
            case GameProgress.LevelStatus.Win:
            case GameProgress.LevelStatus.Lose:
                // Restart current level
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
                break;
        }
    }

    void OnDestroy()
    {
        Instance = null;
    }
}
