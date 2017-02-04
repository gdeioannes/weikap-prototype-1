using UnityEngine;
using System.Linq;

[RequireComponent(typeof(UnityEngine.UI.Text))]
public class LevelMaxValueUpdater : MonoBehaviour {

	public enum Type 
	{
		MaxCoins,
		MaxSamples,
		MaxQuestions
	}

	[SerializeField] Type type;
	UnityEngine.UI.Text textToReplace;

	void Start()
	{
		textToReplace = GetComponent<UnityEngine.UI.Text> ();

		switch (type) 
		{
		case Type.MaxCoins:
				textToReplace.text = PlayerData.Instance.Levels.Values.Sum(s=>s.questions).ToString();
				break;
		case Type.MaxSamples:
				textToReplace.text = PlayerData.Instance.Levels.Values.Sum(s=>s.samples).ToString();
				break;
		case Type.MaxQuestions:
				textToReplace.text = PlayerData.Instance.Levels.Values.Sum(s=>s.questions).ToString ();
				break;		
		}
	}
}