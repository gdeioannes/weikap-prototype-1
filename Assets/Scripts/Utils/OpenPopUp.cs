using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPopUp : MonoBehaviour {

	[SerializeField] [SceneListAttribute] string popUpName;

	public void DoOpen()
	{
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(popUpName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
	}
}
