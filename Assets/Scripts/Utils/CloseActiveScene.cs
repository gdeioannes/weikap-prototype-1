using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseActiveScene : MonoBehaviour {

	public void Close()
	{
		var currentScene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(UnityEngine.SceneManagement.SceneManager.sceneCount - 1);
		UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currentScene);
	}
}
