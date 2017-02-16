using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelsDB", menuName = "Weikap/DB/Levels")]
public class LevelsDBScriptableObject : ScriptableObject 
{
	[System.Serializable]
	public class Level 
	{		
		public string name;
		public int samples;
		public int questions;
		public int coins;
		public Object scene;
		public GameObject levelImage;
	}

	public Level[] levels;

	#if UNITY_EDITOR
	[ContextMenu("GatherInfoFromAllLevels")]
	public void GetInfoFromAllLevels()
	{
		string filePath;
		GameObject[] allSceneRoots;
		List<ConsumableController> allConsumables = new List<ConsumableController> ();
		foreach (var item in levels) 
		{
			filePath = UnityEditor.AssetDatabase.GetAssetPath (item.scene);
			var scene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene (filePath, UnityEditor.SceneManagement.OpenSceneMode.Single);
			allConsumables.Clear ();
			allSceneRoots = scene.GetRootGameObjects ();

			for (int i = 0; i < allSceneRoots.Length; i++) {
				var sceneRoot = allSceneRoots [i];
				var allElements = sceneRoot.GetComponentsInChildren<ConsumableController> ();
				if (allElements != null && allElements.Length > 0) {
					allConsumables.AddRange (sceneRoot.GetComponentsInChildren<ConsumableController> ());
				}
			}

			item.coins = item.questions = item.samples = 0;

			foreach (var consumable in allConsumables) 
			{
				item.coins += consumable.type == InGameItemsDBScriptableObject.ItemType.Coin ? consumable.amount : 0;
				item.coins += consumable.type == InGameItemsDBScriptableObject.ItemType.Question ? (consumable as QuestionZoneController).coinsAfterRightAnswer : 0;
				item.questions += consumable.type == InGameItemsDBScriptableObject.ItemType.Question ? consumable.amount : 0;
				item.samples += consumable.type == InGameItemsDBScriptableObject.ItemType.Sample ? consumable.amount : 0;
			}
		}
	}	

	#endif
}
