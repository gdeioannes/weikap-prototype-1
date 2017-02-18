using System.Collections;
using UnityEngine;

public class SamplesPopUpManager : MonoBehaviour
{
    [SerializeField] [SceneList] string sceneName;
    [SerializeField] bool updateTimeScale;

    bool loadingSamplesPopUp;    
    public void DisplaySamplesPopUp(int sampleId)
    {
        if (!loadingSamplesPopUp)
        {
            StartCoroutine(DisplaySamplesPopUpCoroutine(sampleId));
            loadingSamplesPopUp = true;
        }
    }

    IEnumerator DisplaySamplesPopUpCoroutine(int sampleId)
    {
        var op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        while (!op.isDone)
        {
            yield return null;
        }
        // get last open scene
        var sceneToCheck = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
        var gameRoots = sceneToCheck.GetRootGameObjects();
        SamplesPopUpController samplesPopUpController = null;
        foreach (var item in gameRoots)
        {
            samplesPopUpController = item.GetComponentInChildren<SamplesPopUpController>();
            if (samplesPopUpController != null) { break; }
        }

        if (samplesPopUpController != null)
        {
            samplesPopUpController.Show(sampleId, updateTimeScale);
        }
        loadingSamplesPopUp = false;
    }
}