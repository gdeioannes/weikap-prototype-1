using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Text))]
public class StageProgressValueUpdater : MonoBehaviour 
{
	public enum Type
	{
		MaxCoinsCollected,
		MaxSamplesCollected,
		MaxRightAnsweredQuestions,
		CoinsInLevel,
		SamplesInLevel,
		QuestionsInLevel
	}

	[SerializeField] int levelId;
	[SerializeField] Type type;
	[SerializeField ]bool autoUpdate = true;

	UnityEngine.UI.Text text;

	void Awake()
	{
		text = GetComponent<UnityEngine.UI.Text> ();
	}

	void Start()
	{		
		if (autoUpdate) 
		{
			UpdateValue (this.levelId);
		}
	}

	public void UpdateValue(int levelId)
	{		
		this.levelId = levelId;
		var levelData = GameProgress.Instance.LevelsData.TryGetValue (this.levelId);
		var levelInfo = GameProgress.Instance.Levels.TryGetValue (this.levelId);

		switch (type) 
		{
		case Type.MaxCoinsCollected:
			
			text.text = levelData != null ? levelData.maxCoinsCollected.ToString () : "0";
			break;
		case Type.MaxSamplesCollected:
			text.text = levelData != null ? levelData.maxSamplesCollected.ToString () : "0";
			break;
		case Type.MaxRightAnsweredQuestions:
			text.text = levelData != null ? levelData.maxRightAnsweredQuestions.ToString () : "0";
			break;
		case Type.CoinsInLevel:
			text.text = levelInfo != null ? levelInfo.coins.ToString() : "0";
			break;
		case Type.SamplesInLevel: 
			text.text = levelInfo != null ? levelInfo.samples.ToString() : "0";
			break;
		case Type.QuestionsInLevel:
			text.text = levelInfo != null ? levelInfo.questions.ToString() : "0";
			break;
		}
	}
}
