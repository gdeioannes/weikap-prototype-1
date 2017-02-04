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

    [SerializeField] [SceneListAttribute] string nextSceneName;
    [SerializeField] bool useTimer;
    [Range(0.1f,10f)] [SerializeField] float secondsToNextScene;
    [Range(0.1f, 5f)] [SerializeField] float transitionSeconds;
    [SerializeField] State state;

    void Start()
    {
        if (useTimer) { StartCoroutine(WaitAndGoToNextScene()); }
    }

    IEnumerator WaitAndGoToNextScene()
    {
        state = State.Waiting;
        yield return new WaitForSeconds(secondsToNextScene);
		ToNextScene(this.nextSceneName);
    }

	public void ToNextScene()
	{
		ToNextScene(this.nextSceneName);
	}

	public void ToNextScene(string sceneName)
    {
		this.nextSceneName = sceneName;
		if (state == State.Waiting)
        {
            StopAllCoroutines(); // stop waiting coroutine
        }

        if (state == State.DoingTransition) { return; /*already doing transition */ }

        state = State.DoingTransition;

		FadeTransitionManager.Instance.ToNextScene(sceneName, transitionSeconds);
    }
}
