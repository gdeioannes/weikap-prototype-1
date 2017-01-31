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
			textToReplace.text = GameProgress.Instance.CoinsAvailable.ToString();
			break;
		case Type.SamplesCurrent:
			textToReplace.text = GameProgress.Instance.SamplesCollected.Count.ToString();
			break;
		case Type.QuestionsCurrent:
			textToReplace.text = GameProgress.Instance.QuestionsAnswered.ToString ();
			break;
		case Type.ToolsCurrent:
			textToReplace.text = GameProgress.Instance.ToolsUnlocked.Count.ToString();
			break;
		case Type.CoinsCollected:
			textToReplace.text = GameProgress.Instance.CoinsCollected.ToString();
			break;
		case Type.LevelsCompleted:
			textToReplace.text = GameProgress.Instance.LevelsData.Count(s=>s.Value.status == GameProgress.LevelStatus.Win).ToString();
			break;
		case Type.LevelsLost:
			textToReplace.text = GameProgress.Instance.LevelsData.Count(s=>s.Value.status == GameProgress.LevelStatus.Lose).ToString();
			break;
		case Type.TotalRightAnswers:
			textToReplace.text = GameProgress.Instance.LevelsData.Values.Sum(s=>s.maxRightAnsweredQuestions).ToString();
			break;
		case Type.TotalWrongAnswers:
			textToReplace.text =  (GameProgress.Instance.Levels.Sum(s=>s.questions) - GameProgress.Instance.LevelsData.Sum(s=>s.Value.maxRightAnsweredQuestions)).ToString();
			break;
		case Type.TimePlayed:
			textToReplace.text = System.TimeSpan.FromSeconds (GameProgress.Instance.TotalGameTime).ToString ();
			break;
		default:
			textToReplace.text = "?";
			break;
		}
	}
}
