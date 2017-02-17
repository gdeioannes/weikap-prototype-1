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
    [SerializeField] UnityEngine.UI.Text samplesCurrent;
    [SerializeField] UnityEngine.UI.Text samplesMax;
    
    [SerializeField] SamplesPopUpController samplesPopUpController;
    [SerializeField] QuestionsPopUpController questionController;
    [SerializeField] [SceneList] string samplesPopUpScene;
	[SerializeField] [SceneList] string stageEndedScene;

    public Transform CoinsIconContainer;
    public Transform QuestionsIconContainer;
    public Transform SamplesIconContainer;
    public Camera WorldCamera;
    public RectTransform UIMainRectTransform;

    public float consumablesMoveToUITime = 2f;
    public DG.Tweening.Ease consumableMovEaseType = DG.Tweening.Ease.InCubic;

    public static GameController Instance { get; private set; }
    public SamplesDBScriptableObject.Sample[] SamplesDB { get { return this.samplesDB.samples; } }
	public ToolsDBScriptableObject.Tool[] ToolsDB {get { return this.toolsDB.Tools; }}

    public System.Action OnConsumablesUpdated = delegate {};

    void Awake()
    {
        Instance = this;
        cameraFollower.enabled = false;
        energySlider.value = 100;
		var gameProgressInstance = PlayerData.Instance; // creating game progress instance
    }

    void Start()
    {
        coinsMax.text = PlayerData.Instance.Levels[levelProgress.Id].coins.ToString();
        questionsMax.text = PlayerData.Instance.Levels[levelProgress.Id].questions.ToString();
        samplesMax.text = PlayerData.Instance.Levels[levelProgress.Id].samples.ToString();        
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

		if(type == InGameItemsDBScriptableObject.ItemType.Sample)
		{
			levelProgress.samples.Add(index);
		}

        UpdateConsumablesUI(type, levelProgress.consumables[type]);

        OnConsumablesUpdated();
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
            case InGameItemsDBScriptableObject.ItemType.Sample:
                samplesCurrent.text = amount.ToString();
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

    bool loadingSamplesPopUp;
    public void DisplaySamplesPopUp(int sampleId)
    {
        if ( samplesPopUpController != null) { samplesPopUpController.Show(sampleId); }
        else if (!loadingSamplesPopUp)
        {
            StartCoroutine(DisplaySamplesPopUpCoroutine(sampleId));
            loadingSamplesPopUp = true;
        }
    }

    IEnumerator DisplaySamplesPopUpCoroutine(int sampleId)
    {
        var op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(samplesPopUpScene, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        yield return op;
        // get last open scene
        var sceneToCheck = UnityEngine.SceneManagement.SceneManager.GetSceneByName(samplesPopUpScene);
        var gameRoots = sceneToCheck.GetRootGameObjects();        
        foreach (var item in gameRoots)
        {
            samplesPopUpController = item.GetComponentInChildren<SamplesPopUpController>();
            if (samplesPopUpController != null) {  break; }
        }

        if (samplesPopUpController != null)
        {
            samplesPopUpController.Show(sampleId);
        }
        loadingSamplesPopUp = false;
    }

    public void UpdateLevelProgress(PlayerData.LevelStatus status)
    {
        levelProgress.UpdateGameProgress(status);
        switch (status)
        {
			case PlayerData.LevelStatus.Win:
			case PlayerData.LevelStatus.Lose:
				UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(stageEndedScene, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                break;
        }
    }

    void OnDestroy()
    {
        Instance = null;
    }
}
