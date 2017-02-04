using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SceneListAttribute))]
public class SceneListPropertyDrawer : PropertyDrawer 
{
	List<string> scenesList = null;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		if(property.propertyType != SerializedPropertyType.String)
		{
			base.OnGUI(position, property, label);
			return;
		}

		if(scenesList == null)
		{
			scenesList = new List<string>();
			for(int i = 0, max = UnityEditor.EditorBuildSettings.scenes.Length; i < max; ++i)
			{
				scenesList.Add(System.IO.Path.GetFileNameWithoutExtension(UnityEditor.EditorBuildSettings.scenes[i].path));
			}
		}

		int selectedIndex = Mathf.Max(scenesList.IndexOf(property.stringValue),0);
		selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, scenesList.ToArray());
		property.stringValue = scenesList[selectedIndex];
	}
}
