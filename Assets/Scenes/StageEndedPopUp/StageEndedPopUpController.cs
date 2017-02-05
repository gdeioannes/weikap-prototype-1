using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEndedPopUpController : MonoBehaviour {

	[SerializeField] GameObject stageCompletedContainer;
	[SerializeField] GameObject stageLoseContainer;
	[SerializeField] StageProgressValueUpdater[] stageProgressValues;

	void Awake()
	{
		Time.timeScale = 0;

		var gameStatus = GameController.Instance.levelProgress.Status;

		stageCompletedContainer.SetActive(gameStatus == PlayerData.LevelStatus.Win);
		stageLoseContainer.SetActive(gameStatus == PlayerData.LevelStatus.Lose);

		if(gameStatus == PlayerData.LevelStatus.Win)
		{
			int currentLevel = GameController.Instance.levelProgress.Id;

			foreach(var item in stageProgressValues)
			{
				item.UpdateValue(currentLevel);
			}
		}
	}

	public void SaveCollectedItemsInLevel()
	{
		foreach(var item in GameController.Instance.levelProgress.consumables)
		{
			if (item.Key == InGameItemsDBScriptableObject.ItemType.Coin)
			{
				PlayerData.Instance.UpdateCoinsCollected((long)item.Value);
			}
		}

		foreach(var sampleId in GameController.Instance.levelProgress.samples)
		{
			PlayerData.Instance.UpdateSamplesCollected(sampleId);
		}
	}

	public void RestartCurrentLevel()
	{
		FadeTransitionManager.Instance.ToNextScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, 0.5f);
	}

	void OnDestroy()
	{
		Time.timeScale = 1;
	}
}
