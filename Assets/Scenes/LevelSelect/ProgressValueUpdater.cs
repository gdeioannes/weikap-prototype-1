using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(UnityEngine.UI.Text))]
public class ProgressValueUpdater : MonoBehaviour {

	public enum Type 
	{
		CoinsCurrent,
		SamplesCurrent,
		QuestionsCurrent,
		ToolsCurrent,
		CoinsCollected,
		LevelsCompleted,
		LevelsLost,
		TotalRightAnswers,
		TotalWrongAnswers,
		TimePlayed,
	}

	[SerializeField] Type type;
	UnityEngine.UI.Text textToReplace;

	void Start()
	{
		textToReplace = GetComponent<UnityEngine.UI.Text> ();

		switch (type) 
		{
			case Type.CoinsCurrent:
				textToReplace.text = PlayerData.Instance.CoinsAvailable.ToString();
				break;
			case Type.SamplesCurrent:
				textToReplace.text = PlayerData.Instance.LevelsData.Values.Sum(s=>s.maxSamplesCollected).ToString();
				break;
			case Type.QuestionsCurrent:
                textToReplace.text = PlayerData.Instance.LevelsData.Values.Sum(w => w.maxRightAnsweredQuestions).ToString();
				break;
			case Type.ToolsCurrent:
				textToReplace.text = PlayerData.Instance.ToolsUnlocked.Count.ToString();
				break;
			case Type.CoinsCollected:
				textToReplace.text = PlayerData.Instance.CoinsCollected.ToString();
				break;
			case Type.LevelsCompleted:
				textToReplace.text = PlayerData.Instance.LevelsData.Count(s=>s.Value.status == PlayerData.LevelStatus.Win).ToString();
				break;
			case Type.LevelsLost:
				textToReplace.text = PlayerData.Instance.LevelsData.Count(s=>s.Value.status == PlayerData.LevelStatus.Lose).ToString();
				break;
			case Type.TotalRightAnswers:
				textToReplace.text = PlayerData.Instance.LevelsData.Values.Sum(s=>s.maxRightAnsweredQuestions).ToString();
				break;
			case Type.TotalWrongAnswers:
				textToReplace.text =  (PlayerData.Instance.Levels.Values.Sum(s=>s.questions) - PlayerData.Instance.LevelsData.Sum(s=>s.Value.maxRightAnsweredQuestions)).ToString();
				break;
			case Type.TimePlayed:
				textToReplace.text = System.TimeSpan.FromSeconds (PlayerData.Instance.TotalGameTime).ToString ();
				break;
			default:
				textToReplace.text = "?";
				break;
		}
	}
}
