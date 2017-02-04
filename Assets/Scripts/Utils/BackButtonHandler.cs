using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BackButtonHandler : MonoBehaviour {

	UnityEngine.SceneManagement.Scene currentScene;

	void Awake()
	{
		currentScene = GetLastOpenScene;
	}

	[SerializeField] UnityEvent onBackButtonAction = new UnityEvent();

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape) && GetLastOpenScene.buildIndex == currentScene.buildIndex)
		{
			onBackButtonAction.Invoke();
		}
	}

	UnityEngine.SceneManagement.Scene GetLastOpenScene
	{
		get
		{
			return UnityEngine.SceneManagement.SceneManager.GetSceneAt(UnityEngine.SceneManagement.SceneManager.sceneCount - 1);
		}
	}
}
