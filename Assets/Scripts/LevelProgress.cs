using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelProgress : MonoBehaviour {    

    public Dictionary<InGameItemsDBScriptableObject.ItemType, int> consumables;    
    public int Id { get; private set; }
	PlayerData.LevelStatus status;

    void Awake()
    {
        consumables = new Dictionary<InGameItemsDBScriptableObject.ItemType, int>();
		this.status = PlayerData.LevelStatus.OnGoing;
    }

	public void UpdateGameProgress(PlayerData.LevelStatus status)
    {
        this.status = status;
		PlayerData.Instance.UpdateLevelProgress(Id, consumables, this.status);
    }
}
