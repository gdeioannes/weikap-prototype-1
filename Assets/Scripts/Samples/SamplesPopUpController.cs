using UnityEngine;
using System.Collections;

public class SamplesPopUpController : MonoBehaviour {

    [SerializeField] SamplesListIconController listIconPrefab;
    [SerializeField] GameObject samplesListContainer;
    [SerializeField] UnityEngine.UI.Text selectedSampleName;
    [SerializeField] UnityEngine.UI.Text selectedSampleDesc;
    [SerializeField] UnityEngine.UI.RawImage collectedSelectedSampleImage;
    [SerializeField] UnityEngine.UI.RawImage nonCollectedSelectedSampleImage;

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
        var samplesList = GameController.Instance.SamplesDB;        

        samplesListContainer.transform.DestroyChildren();

        for (int i = 0; i < samplesList.Length; ++i)
        {
            var newSampleItem = Object.Instantiate<SamplesListIconController>(listIconPrefab);
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
}
