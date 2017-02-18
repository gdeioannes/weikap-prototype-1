using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SamplesPopUpController : MonoBehaviour {

    [SerializeField] SamplesListIconController samplesListIconPrefab;
    [SerializeField] GameObject samplesListContainer;
	[SerializeField] ToggleGroup samplesToggleGroup;

    [SerializeField] ToolsListIconController toolsListIconPrefab;
    [SerializeField] GameObject toolsListContainer;
	[SerializeField] ToggleGroup toolsToggleGroup;

    [SerializeField] Text coinsAmount;
    [SerializeField] Text selectedSampleName;
    [SerializeField] Text selectedSampleDesc;
	[SerializeField] Text selectedSampleToolInfo;
    [SerializeField] RawImage collectedSelectedSampleImage;
    [SerializeField] RawImage nonCollectedSelectedSampleImage;

	[SerializeField] ToolsPurchasePopUpController toolsPurchasePopUp;

    [SerializeField] SamplesDBScriptableObject samplesDb;
    [SerializeField] ToolsDBScriptableObject toolsDb;

    bool handlersAdded;

	int selectedSampleId = 0, selectedToolId = -1;
    bool updateTimeScale;
    UnityEngine.SceneManagement.Scene currentScene;

    void Awake()
    {
        currentScene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(UnityEngine.SceneManagement.SceneManager.sceneCount - 1);        
    }

    public void Show(int sampleId, bool updateTimeScale)
    {
		selectedSampleId = sampleId;
		selectedToolId = -1;
        this.updateTimeScale = updateTimeScale;

        if (updateTimeScale) { Time.timeScale = 0; }
		
        Initialize();
		OnSelectSample(selectedSampleId);
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        if (updateTimeScale)
        {
            Time.timeScale = 1;
        }
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currentScene);
    }

    void Initialize()
    {
        if (!handlersAdded)
        {
			PlayerData.Instance.OnCoinsAmountUpdated += UpdateCoinsAvailable;
			this.coinsAmount.text = PlayerData.Instance.CoinsAvailable.ToString();
            handlersAdded = true;
        }

        InitializeToolsList();
        InitializeSamplesList();
    }

    void UpdateCoinsAvailable(long amount)
    {
        this.coinsAmount.text = amount.ToString();
    }

    void InitializeSamplesList()
    {
        var samplesList = samplesDb.samples;

        samplesListContainer.transform.DestroyChildren();

        for (int i = 0; i < samplesList.Length; ++i)
        {
            var newSampleItem = Object.Instantiate<SamplesListIconController>(samplesListIconPrefab);
            newSampleItem.transform.SetParent(this.samplesListContainer.transform, false);

            if (i == selectedSampleId)
            {
                newSampleItem.GetComponent<Toggle>().isOn = true;
            }

            newSampleItem.Set(i, samplesDb.samples[i], samplesToggleGroup);
            newSampleItem.onSelectCb = OnSelectSample;
        }
    }

	void InitializeToolsList()
	{
		var toolsList = toolsDb.Tools;
		toolsListContainer.transform.DestroyChildren();

		for(int i = 0; i < toolsList.Length; ++i)
		{
			var newToolItem = Object.Instantiate<ToolsListIconController>(toolsListIconPrefab);
			newToolItem.transform.SetParent(this.toolsListContainer.transform, false);
			newToolItem.Set(i, toolsToggleGroup);
			newToolItem.onSelectCb = OnSelectTool;
		}
	}

    void OnSelectSample(int sampleId)
    {
		selectedSampleId = sampleId;
		var selectedSample = samplesDb.samples[selectedSampleId];
		bool collectedStatus = PlayerData.Instance.SamplesCollected.Contains(selectedSampleId);
        selectedSampleName.text = selectedSample.Name;
        selectedSampleDesc.text = selectedSample.Description;
        collectedSelectedSampleImage.texture = selectedSample.Image;
        nonCollectedSelectedSampleImage.texture = selectedSample.Image;
        collectedSelectedSampleImage.color = selectedSample.ImageColor;
        collectedSelectedSampleImage.enabled = collectedStatus;
        nonCollectedSelectedSampleImage.enabled = !collectedStatus;
        ShowSelectedToolInfo();
    }

	void OnSelectTool(int toolId)
	{
        selectedToolId = toolId;

        if (selectedToolId < 0) { selectedSampleToolInfo.text = string.Empty; return; } // invalid tool id

		bool unlockStatus = PlayerData.Instance.ToolsUnlocked.Contains(selectedToolId);

		// get current tool status
		if (!unlockStatus)
		{
			// try buy current selected tool
			toolsPurchasePopUp.Show(selectedToolId, toolsDb.Tools[selectedToolId]);
            selectedSampleToolInfo.text = string.Empty;
            return;
		}
        
        ShowSelectedToolInfo(); // show tool related info
    }

    void ShowSelectedToolInfo()
    {
        if (selectedToolId < 0) { selectedSampleToolInfo.text = string.Empty; return; } // invalid tool id
        bool unlockStatus = PlayerData.Instance.ToolsUnlocked.Contains(selectedToolId);
        bool sampleUnlocked = PlayerData.Instance.SamplesCollected.Contains(selectedSampleId);

        if (unlockStatus && sampleUnlocked)
        {
            // show tool related info
            var selectedSample = samplesDb.samples[selectedSampleId];
            selectedSampleToolInfo.text = selectedSample.GetToolUnlockInfo(selectedToolId);
        }
        else
        {
            selectedSampleToolInfo.text = string.Empty;
        }
    }    

    void OnDestroy()
    {
        if (handlersAdded)
        {
			if (PlayerData.Instance != null)
            {
				PlayerData.Instance.OnCoinsAmountUpdated -= UpdateCoinsAvailable;
            }
        }
    }
}
