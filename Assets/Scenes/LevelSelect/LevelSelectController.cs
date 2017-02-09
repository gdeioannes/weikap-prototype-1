using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectController : MonoBehaviour 
{	
	[SerializeField] UnityEngine.UI.Text levelName;
	[SerializeField] Transform levelImageContainer;
	[SerializeField] StageProgressValueUpdater[] valueControllers;

	int levelId;
	System.Action<int> onLevelSelectCb;

	public void Initialize(int levelId, System.Action<int> onLevelSelectCb)
	{
		this.levelId = levelId;
		var levelDef = PlayerData.Instance.Levels.TryGetValue (this.levelId);
		this.levelName.text = levelDef.name;
		this.onLevelSelectCb = onLevelSelectCb;
		UpdateLevelImage ();

		foreach (var item in valueControllers) 
		{
			item.UpdateValue (this.levelId);
		}
	}

	void UpdateLevelImage()
	{
		levelImageContainer.DestroyChildren (); // destroy all children
		GameObject newLevelImage = Object.Instantiate<GameObject>(PlayerData.Instance.Levels[this.levelId].levelImage);
		newLevelImage.transform.SetParent (levelImageContainer, false);
	}

	public void OnLevelSelect()
	{
		this.onLevelSelectCb (this.levelId);
	}
}
