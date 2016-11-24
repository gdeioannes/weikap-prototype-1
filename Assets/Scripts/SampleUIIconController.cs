using UnityEngine;
using System.Collections;

public class SampleUIIconController : MonoBehaviour {

    [SerializeField] UnityEngine.UI.Image icon;
    [SerializeField] UnityEngine.UI.Image nonCollectedIcon;
    int sampleId;

    public void Set(int sampleId)
    {
        this.sampleId = sampleId;
        SamplesDBScriptableObject.Sample sample = GameController.Instance.samplesDB.samples[sampleId];
        bool collectedStatus = GameProgress.Instance.SamplesCollected.Contains(sampleId);                

        icon.sprite = sample.Icon;
        icon.color = sample.IconColor;
        nonCollectedIcon.sprite = sample.Icon;
        nonCollectedIcon.enabled = !collectedStatus;

        GameProgress.Instance.OnSamplesCollectionUpdated += OnSamplesCollectionUpdatedHandler;
    }

    void OnSamplesCollectionUpdatedHandler()
    {
        bool collectedStatus = GameProgress.Instance.SamplesCollected.Contains(sampleId);
        nonCollectedIcon.enabled = !collectedStatus;
    }

    public void OnClick()
    {
        // show samples UI
    }
}
