using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeToNextScene : MonoBehaviour {

    public enum State
    {
        Idle,
        Waiting,
        DoingTransition
    }

    [SerializeField] string nextSceneName;
    [SerializeField] bool useTimer;
    [Range(0.1f,10f)] [SerializeField] float secondsToNextScene;
    [SerializeField] CanvasGroup canvasGroup;
    [Range(0.1f, 5f)] [SerializeField] float transitionSeconds;
    [SerializeField] State state;

    void Start()
    {
        if (canvasGroup == null) { canvasGroup = this.GetComponentInChildren<CanvasGroup>(); }
        if (useTimer) { StartCoroutine(WaitAndGoToNextScene()); }
    }

    IEnumerator WaitAndGoToNextScene()
    {
        state = State.Waiting;
        yield return new WaitForSeconds(secondsToNextScene);
        ToNextScene();
    }

    public void ToNextScene()
    {
        if (state == State.Waiting)
        {
            StopAllCoroutines(); // stop waiting coroutine
        }

        if (state == State.DoingTransition) { return; /*already doing transition */ }

        state = State.DoingTransition;
        StartCoroutine(TransitionToNextScene());
    }

    IEnumerator TransitionToNextScene()
    {
        // begin loading next scene
        var op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(nextSceneName);
        op.allowSceneActivation = false;

        var tweener = canvasGroup.DOFade(0, transitionSeconds);
        yield return tweener.WaitForCompletion();
        yield return new WaitForSeconds(0.1f); // extra wait time


        op.allowSceneActivation = true;
    }
}
