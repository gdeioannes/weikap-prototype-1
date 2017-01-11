using UnityEngine;
using System.Collections;

public class SamplesPopUpController : MonoBehaviour {

    [SerializeField] SamplesListIconController samplesListIconPrefab;
    [SerializeField] GameObject samplesListContainer;

    [SerializeField] ToolsListIconController toolsListIconPrefab;
    [SerializeField] GameObject toolsListContainer;

    [SerializeField] UnityEngine.UI.Text coinsAmount;
    [SerializeField] UnityEngine.UI.Text selectedSampleName;
    [SerializeField] UnityEngine.UI.Text selectedSampleDesc;
    [SerializeField] UnityEngine.UI.RawImage collectedSelectedSampleImage;
    [SerializeField] UnityEngine.UI.RawImage nonCollectedSelectedSampleImage;

    bool handlersAdded;

    public void Show(int sampleId)
    {
        Time.timeScale = 0;
        Initialize();
        OnSelect(sampleId);
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    void Initialize()
    {
        if (!handlersAdded)
        {
            GameProgress.Instance.OnCoinsAmountUpdated += UpdateCoinsAvailable;
            this.coinsAmount.text = GameProgress.Instance.CoinsAvailable.ToString();
            handlersAdded = true;
        }

        InitializeSamplesList();
    }

    void UpdateCoinsAvailable(long amount)
    {
        this.coinsAmount.text = amount.ToString();
    }

    void InitializeSamplesList()
    {
        var samplesList = GameController.Instance.SamplesDB;

        samplesListContainer.transform.DestroyChildren();

        for (int i = 0; i < samplesList.Length; ++i)
        {
            var newSampleItem = Object.Instantiate<SamplesListIconController>(samplesListIconPrefab);
            newSampleItem.transform.SetParent(this.samplesListContainer.transform, false);
            newSampleItem.Set(i);
            newSampleItem.onSelectCb = OnSelect;
        }
    }

    void OnSelect(int sampleId)
    {
        var selectedSample = GameController.Instance.SamplesDB[sampleId];
        bool collectedStatus = GameProgress.Instance.SamplesCollected.Contains(sampleId);
        selectedSampleName.text = selectedSample.Name;
        selectedSampleDesc.text = selectedSample.Description;
        collectedSelectedSampleImage.texture = selectedSample.Image;
        nonCollectedSelectedSampleImage.texture = selectedSample.Image;
        collectedSelectedSampleImage.color = selectedSample.ImageColor;
        collectedSelectedSampleImage.enabled = collectedStatus;
        nonCollectedSelectedSampleImage.enabled = !collectedStatus;
    }

    void OnDestroy()
    {
        if (handlersAdded)
        {
            if (GameProgress.Instance != null)
            {
                GameProgress.Instance.OnCoinsAmountUpdated -= UpdateCoinsAvailable;
            }
        }
    }
}
