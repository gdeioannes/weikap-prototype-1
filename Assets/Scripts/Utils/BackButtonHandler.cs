using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BackButtonHandler : MonoBehaviour {

	UnityEngine.SceneManagement.Scene currentScene;    
	[SerializeField] bool onlyWhenTimeScale1 = false;

	void Awake()
	{
		currentScene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(UnityEngine.SceneManagement.SceneManager.sceneCount-1);
	}

	[SerializeField] UnityEvent onBackButtonAction = new UnityEvent();

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape) && GetLastOpenScene.buildIndex == currentScene.buildIndex)
		{
			if (!onlyWhenTimeScale1 || (onlyWhenTimeScale1 && Time.timeScale == 1))
			{
				onBackButtonAction.Invoke();
			}
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
