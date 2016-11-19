using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    [SerializeField] TwoDCameraFollower cameraFollower;
    [SerializeField] LevelContainer levelContainer;
    [SerializeField] CharacterControl characterPrefab;
    [SerializeField] float energyDecreaseInterval = 1f;

    [Header("UI Elements")]
    [SerializeField] UnityEngine.UI.Slider energySlider;    
    [SerializeField] UnityEngine.UI.Text coinsCurrent;
    [SerializeField] UnityEngine.UI.Text coinsMax;
    [SerializeField] UnityEngine.UI.Text samplesCurrent;
    [SerializeField] UnityEngine.UI.Text samplesMax;
    [SerializeField] UnityEngine.UI.Text questionsCurrent;
    [SerializeField] UnityEngine.UI.Text questionsMax;

    public Transform CoinsIconContainer;    
    public Transform SamplesIconContainer;
    public Transform QuestionsIconContainer;
    public Camera WorldCamera;
    public RectTransform UIMainRectTransform;

    public float consumablesMoveToUITime = 2f;
    public DG.Tweening.Ease consumableMovEaseType = DG.Tweening.Ease.InCubic;

    public static GameController Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        cameraFollower.enabled = false;
        energySlider.value = 100;
    }

    void Start()
    {
        coinsMax.text = levelContainer.CoinsCount.ToString();
        samplesMax.text = levelContainer.SamplesCount.ToString();
        questionsMax.text = levelContainer.QuestionsCount.ToString();
        SpawnCharacterOnMap();
    }

    void SpawnCharacterOnMap()
    {
        CharacterControl character = Object.Instantiate<CharacterControl>(characterPrefab);
        character.OnConsumableAmountUpdated += OnConsumableAmountUpdated;
        character.OnEnergyUpdated += OnEnergyUpdated;
        character.OnQuestionAnswered += OnQuestionAnswered;
        character.StartCoroutine(character.EnergyCountDownCoroutine(energyDecreaseInterval));
        Transform spawnPoint = levelContainer.spawnPoints[Random.Range(0, levelContainer.spawnPoints.Length-1)].transform;
        character.gameObject.transform.SetParent(levelContainer.transform, false);
        character.gameObject.transform.position = spawnPoint.position;
        cameraFollower.target = character.gameObject.transform;
        cameraFollower.enabled = true;
    }

    void OnConsumableAmountUpdated(ConsumableController.ConsumableType type, int amount)
    {
        switch (type)
        {
            case ConsumableController.ConsumableType.Coin:
                coinsCurrent.text = amount.ToString();                
                break;
            case ConsumableController.ConsumableType.Sample:
                samplesCurrent.text = amount.ToString();
                break;
        }
    }

    void OnEnergyUpdated(float amount)
    {
        energySlider.value = amount;        
    }

    void OnQuestionAnswered(int amount)
    {
        questionsCurrent.text = amount.ToString();
    }

    void OnDestroy()
    {
        Instance = null;
    }
}
