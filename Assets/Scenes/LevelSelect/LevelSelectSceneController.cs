using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectSceneController : MonoBehaviour {

	[SerializeField] LevelSelectController levelSelectBoxPrefab;
	[SerializeField] Transform levelSelectListContainer;
	[SerializeField] FadeToNextScene toNextSceneController;

	void Start()
	{
		levelSelectListContainer.DestroyChildren (); // destroy all container children

		var enumerator = PlayerData.Instance.Levels.Values.GetEnumerator ();
		int levelIndex = 0;
		while (enumerator.MoveNext ()) 
		{
			var newLevelSelectBox = Object.Instantiate<LevelSelectController> (levelSelectBoxPrefab);
			newLevelSelectBox.transform.SetParent (levelSelectListContainer, false);
			newLevelSelectBox.gameObject.layer = levelSelectListContainer.gameObject.layer;
			newLevelSelectBox.Initialize (levelIndex, OnLevelSelect);
			++levelIndex;
		}
	}

	void OnLevelSelect(int levelId)
	{
		string levelName = PlayerData.Instance.Levels [levelId].scene.name;
		toNextSceneController.ToNextScene (levelName);
	}
}
