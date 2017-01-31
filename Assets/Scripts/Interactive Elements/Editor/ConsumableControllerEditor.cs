using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(ConsumableController))]
public class ConsumableControllerEditor : BaseInteractiveElementEditor
{
    ConsumableController controller;
    SamplesDBScriptableObject samplesDB;
	QuestionsDBScriptableObject questionsDB;
    List<string> sampleNamesList;
	List<string> questionsNamesList;

    protected override void OnEnable()
    {
        base.OnEnable();
        controller = target as ConsumableController;
        GetSamplesDB();
		GetQuestionsDB ();
    }

    void GetSamplesDB()
    {
        // Get SamplesDB
        var assetsFindResult = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(SamplesDBScriptableObject).FullName));
        if (assetsFindResult == null || assetsFindResult.Length == 0)
        {
            return;
        }

        var path = AssetDatabase.GUIDToAssetPath(assetsFindResult[0]);
        samplesDB = AssetDatabase.LoadAssetAtPath<SamplesDBScriptableObject>(path);
        sampleNamesList = samplesDB.samples.Select(s => s.Name).ToList();
    }

	void GetQuestionsDB()
	{
		var assetsFindResult = AssetDatabase.FindAssets (string.Format ("t:{0}", typeof(QuestionsDBScriptableObject).FullName));
		if (assetsFindResult == null || assetsFindResult.Length == 0) 
		{
			return;
		}

		var path = AssetDatabase.GUIDToAssetPath (assetsFindResult [0]);
		questionsDB = AssetDatabase.LoadAssetAtPath<QuestionsDBScriptableObject> (path);
		questionsNamesList = questionsDB.questions.Select (s => s.Name).ToList ();
	}

    public override void OnInspectorGUI()
    {
        controller.type = (InGameItemsDBScriptableObject.ItemType)EditorGUILayout.EnumPopup("Tipo", controller.type);
		if (controller.type == InGameItemsDBScriptableObject.ItemType.Coin) 
		{
			controller.amount = EditorGUILayout.IntField ("Cantidad", controller.amount);
		} 
		else if (controller.type == InGameItemsDBScriptableObject.ItemType.Sample) 
		{
			if (sampleNamesList == null || sampleNamesList.Count == 0) 
			{
				EditorGUILayout.HelpBox ("Debes crear una base de datos de muestras antes de continuar.\nLa base de datos debe tener al menos un elemento", MessageType.Error);
			} 
			else 
			{
				int selectedSample = Mathf.Clamp (controller.id, 0, sampleNamesList.Count);
				controller.id = EditorGUILayout.Popup ("Muestra", selectedSample, sampleNamesList.ToArray ());
				UpdateSelectedSample (samplesDB.samples [controller.id]);
			}
		} 
		else if (controller.type == InGameItemsDBScriptableObject.ItemType.Question) 
		{
			if (questionsNamesList == null || questionsNamesList.Count == 0) 
			{
				EditorGUILayout.HelpBox ("Debes crear una base de datos de preguntas antes de continuar.\nLa base de datos debe tener al menos un elemento", MessageType.Error);
			} 
			else 
			{
				int selectedQuestion = Mathf.Clamp (controller.id, 0, questionsNamesList.Count);
				controller.id = EditorGUILayout.Popup ("Pregunta", selectedQuestion, questionsNamesList.ToArray ());
			}
		}

        DrawShapeSelector();
    }

    void UpdateSelectedSample(SamplesDBScriptableObject.Sample sample)
    {
        SpriteRenderer spriteRenderer = controller.GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = sample.Icon;
        spriteRenderer.color = sample.IconColor;
    }
}
