using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "NewSamplesDB", menuName = "Weikap/DB/Samples")]
public class SamplesDBScriptableObject : ScriptableObject {

    [System.Serializable]
    public struct Sample
    {
        public string Name;
        public string Description;

        public Sprite Icon;
        public Color IconColor;
        public Texture Image;
        public Color ImageColor;

		public ToolUnlockInfo[] toolUnlockInfo;

		public string GetToolUnlockInfo(int tooldId)
		{
			foreach(var item in toolUnlockInfo)
			{
				if(item.toolId == tooldId)
				{
					return item.info;
				}
			}
			return string.Empty;
		}
    }

	[System.Serializable]
	public struct ToolUnlockInfo
	{
		[ToolsListAttribute] public int toolId;
		public string info;
	}

    public Sample[] samples;
}
