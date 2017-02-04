using UnityEngine;
using System.Collections;

public class SampleUIIconController : MonoBehaviour {

    [SerializeField] UnityEngine.UI.Image icon;
    [SerializeField] UnityEngine.UI.Image nonCollectedIcon;

    protected int sampleId;
    protected SamplesDBScriptableObject.Sample sample;
    protected bool collectedStatus;

    private bool handlerAdded = false;

    public void Set(int sampleId)
    {
        this.sampleId = sampleId;
        sample = GameController.Instance.SamplesDB[sampleId];
		collectedStatus = PlayerData.Instance.SamplesCollected.Contains(sampleId);

        UpdateUI();

        if (!handlerAdded)
        {
			PlayerData.Instance.OnSamplesCollectionUpdated += OnSamplesCollectionUpdatedHandler;            
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
