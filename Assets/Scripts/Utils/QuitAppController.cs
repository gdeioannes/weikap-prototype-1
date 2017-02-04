using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitAppController : MonoBehaviour {

	public void DoQuit()
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}
