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

    bool handlersAdded;

	int selectedSampleId = 0, selectedToolId = -1;

    public void Show(int sampleId)
    {
		selectedSampleId = sampleId;
		selectedToolId = -1;
		Time.timeScale = 0;
        Initialize();
		OnSelectSample(selectedSampleId);
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
			PlayerData.Instance.OnCoinsAmountUpdated += UpdateCoinsAvailable;
			this.coinsAmount.text = PlayerData.Instance.CoinsAvailable.ToString();
            handlersAdded = true;
        }

        InitializeSamplesList();
		InitializeToolsList();
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
			newSampleItem.onSelectCb = OnSelectSample;
        }
    }

	void InitializeToolsList()
	{
		var toolsList = GameController.Instance.ToolsDB;
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
		var selectedSample = GameController.Instance.SamplesDB[selectedSampleId];
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

		if(selectedToolId < 0) { selectedSampleToolInfo.text = string.Empty; return; } // invalid tool id

		bool unlockStatus = PlayerData.Instance.ToolsUnlocked.Contains(selectedToolId);

		// get current tool status
		if (!unlockStatus)
		{
			// try buy current selected tool
			toolsPurchasePopUp.Show(toolId);
			return;
		}

		// show tool related info
		var selectedSample = GameController.Instance.SamplesDB[selectedSampleId];
		selectedSampleToolInfo.text = selectedSample.GetToolUnlockInfo(selectedToolId);
	}

    void ShowSelectedToolInfo()
    {
        if (selectedToolId < 0) { selectedSampleToolInfo.text = string.Empty; return; } // invalid tool id
        bool unlockStatus = PlayerData.Instance.ToolsUnlocked.Contains(selectedToolId);

        if (unlockStatus)
        {
            // show tool related info
            var selectedSample = GameController.Instance.SamplesDB[selectedSampleId];
            selectedSampleToolInfo.text = selectedSample.GetToolUnlockInfo(selectedToolId);
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
