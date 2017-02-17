using UnityEngine;
using System.Collections;

public class SampleUIIconController : MonoBehaviour {

    [SerializeField] UnityEngine.UI.Image icon;
    [SerializeField] UnityEngine.UI.Image nonCollectedIcon;

    protected int sampleId;
    protected SamplesDBScriptableObject.Sample sample;
    protected bool collectedStatus;

    protected bool handlerAdded { get; private set; }

    public void Set(int id)
    {
        this.sampleId = id;
        sample = GameController.Instance.SamplesDB[id];
		collectedStatus = PlayerData.Instance.SamplesCollected.Contains(id);

        UpdateUI();

        if (!handlerAdded)
        {
            if (GameController.Instance != null)
            {
                GameController.Instance.OnConsumablesUpdated += OnConsumablesUpdatedHandler;
            }
            else
            {
                PlayerData.Instance.OnSamplesCollectionUpdated += OnSamplesCollectionUpdatedHandler;
            }
            
            handlerAdded = true;
        }
    }

    protected virtual void UpdateUI()
    {
        icon.sprite = sample.Icon;
        icon.color = sample.IconColor;
        nonCollectedIcon.sprite = sample.Icon;
        icon.enabled = collectedStatus;
        nonCollectedIcon.enabled = !collectedStatus;
    }

    void OnSamplesCollectionUpdatedHandler()
    {
        collectedStatus = PlayerData.Instance.SamplesCollected.Contains(sampleId);
        icon.enabled = collectedStatus;
        nonCollectedIcon.enabled = !collectedStatus;
    }

    void OnConsumablesUpdatedHandler()
    {
        collectedStatus = GameController.Instance.levelProgress.samples.Contains(sampleId);
        icon.enabled = collectedStatus;
        nonCollectedIcon.enabled = !collectedStatus;
    }

    public virtual void OnSelect()
    {
        GameController.Instance.DisplaySamplesPopUp(this.sampleId);
    }

    void OnDestroy()
    {
        if (handlerAdded)
        {
			if (PlayerData.Instance != null)
            {
				PlayerData.Instance.OnSamplesCollectionUpdated -= OnSamplesCollectionUpdatedHandler;
            }
        }        
    }
}
