using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseActiveScene : MonoBehaviour {

    UnityEngine.SceneManagement.Scene currentScene;
    void Awake()
    {
        currentScene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(UnityEngine.SceneManagement.SceneManager.sceneCount - 1);
    }

    public void Close()
	{
		UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currentScene);
	}
}
