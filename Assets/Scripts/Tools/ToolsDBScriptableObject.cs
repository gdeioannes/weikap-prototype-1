using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSamplesDB", menuName = "Weikap/DB/Tools")]
public class ToolsDBScriptableObject : ScriptableObject
{
    [System.Serializable]
    public struct Tool
    {
        public string name;
        public string description;
        public int unlockCost;
    }
}
