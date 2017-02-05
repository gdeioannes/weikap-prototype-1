using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ToolsListAttribute))]
public class ToolsListPropertyDrawer : PropertyDrawer 
{
	List<string> toolsList = null;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		if(property.propertyType != SerializedPropertyType.Integer)
		{
			base.OnGUI(position, property, label);
			return;
		}

		if(toolsList == null)
		{
			toolsList = new List<string>();
			// get tools db
			var results = AssetDatabase.FindAssets(string.Format("t:{0}",typeof(ToolsDBScriptableObject).FullName));
			if(results == null || results.Length == 0)
			{
				EditorGUI.HelpBox(position, "Valid ToolsDB not found", MessageType.Error);
				return;
			}
			string assetPath = AssetDatabase.GUIDToAssetPath(results[0]);
			var toolsDB = AssetDatabase.LoadAssetAtPath<ToolsDBScriptableObject>(assetPath);

			foreach(var item in toolsDB.Tools)
			{
				toolsList.Add(item.name);
			}
		}

		property.intValue = Mathf.Clamp(property.intValue, 0, toolsList.Count - 1);
		property.intValue = EditorGUI.Popup(position, label.text, property.intValue, toolsList.ToArray());
	}
}
