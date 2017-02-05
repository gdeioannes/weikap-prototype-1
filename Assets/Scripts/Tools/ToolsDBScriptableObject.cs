using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewToolsDB", menuName = "Weikap/DB/Tools")]
public class ToolsDBScriptableObject : ScriptableObject
{
    [System.Serializable]
    public struct Tool
    {
        public string name;
        public string description;
		public Sprite icon;
        public int unlockCost;
    }

	public Tool[] Tools;
}
