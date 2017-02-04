using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[Prefab]
public class FadeTransitionManager : Singleton<FadeTransitionManager> 
{
	public enum State
	{
		Idle,
		DoingTransition
	}

	[SerializeField] CanvasGroup canvasGroup;
	[SerializeField] [SceneListAttribute] string nextSceneName;

	[SerializeField] bool doFadeOut;
	[Range(0.1f, 5f)] [SerializeField] float fadeOutSeconds;
	[SerializeField] bool doFadeIn;
	[Range(0.1f, 5f)] [SerializeField] float fadeInSeconds;
	[SerializeField] State state;

	void Awake()
	{
		if (canvasGroup == null) { canvasGroup = this.GetComponentInChildren<CanvasGroup>(); }
		canvasGroup.alpha = 0;
		canvasGroup.gameObject.SetActive(false);
		Object.DontDestroyOnLoad(this.gameObject);
	}

	public void ToNextScene(string sceneName, float fadeInSeconds = 0.1f, float fadeOutSeconds = 0.1f)
	{
		this.fadeOutSeconds = fadeOutSeconds; this.fadeInSeconds = fadeInSeconds;

		this.nextSceneName = sceneName;
		if (state == State.DoingTransition) { return; /*already doing transition */ }

		state = State.DoingTransition;
		StartCoroutine(TransitionToNextScene());
	}

	IEnumerator TransitionToNextScene()
	{
		canvasGroup.gameObject.SetActive(true);

		if(doFadeOut)
		{
			canvasGroup.alpha = 0;
			var tweener = canvasGroup.DOFade(1, fadeOutSeconds);
			yield return tweener.WaitForCompletion();
			canvasGroup.alpha = 1;
		}

		// begin loading next scene
		var op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(nextSceneName);
		op.allowSceneActivation = false;

		yield return new WaitForSeconds(0.1f); // extra wait time

		op.allowSceneActivation = true;

		if(doFadeIn)
		{
			canvasGroup.alpha = 1;
			var tweener = canvasGroup.DOFade(0, fadeInSeconds);
			yield return tweener.WaitForCompletion();
			canvasGroup.alpha = 0;
		}
		state = State.Idle;
		canvasGroup.gameObject.SetActive(false);
	}
}
