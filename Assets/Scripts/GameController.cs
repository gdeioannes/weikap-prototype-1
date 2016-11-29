using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    [Header("Config")]

    public QuestionsDBScriptableObject questionsDB;    
    public LevelProgress levelProgress;
    [SerializeField] SamplesDBScriptableObject samplesDB;
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
        GameProgress.Instantiate();
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

    public void UpdateConsumable(ConsumableController.ConsumableType type, int index, int amount)
    {
        if (levelProgress.consumables.ContainsKey(type))
        {
            levelProgress.consumables[type] += amount;
        }
        else
        {
            levelProgress.consumables[type] = amount;
        }

        if (type == ConsumableController.ConsumableType.Sample)
        {
            GameProgress.Instance.UpdateSamplesCollected(index);
        }

        UpdateConsumablesUI(type, levelProgress.consumables[type]);
    }

    void UpdateConsumablesUI(ConsumableController.ConsumableType type, int amount)
    {
        switch (type)
        {
            case ConsumableController.ConsumableType.Coin:
                coinsCurrent.text = amount.ToString();                
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

    public void UpdateQuestion(int amount)
    {
        levelProgress.answeredQuestions += amount;
        questionsCurrent.text = levelProgress.answeredQuestions.ToString();
    }

    public Transform GetSampleIconContainer(int sampleId)
    {
        return samplesUIController.GetSampleIconTransform(sampleId);
    }

    void OnDestroy()
    {
        Instance = null;
    }
}
