using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelProgress : MonoBehaviour {    

    public Dictionary<InGameItemsDBScriptableObject.ItemType, int> consumables;    
    public int Id { get; private set; }
	public PlayerData.LevelStatus Status { get; private set; }

    void Awake()
    {
        consumables = new Dictionary<InGameItemsDBScriptableObject.ItemType, int>();
		this.Status = PlayerData.LevelStatus.OnGoing;
    }

	public void UpdateGameProgress(PlayerData.LevelStatus status)
    {
        this.Status = status;
		PlayerData.Instance.UpdateLevelProgress(Id, consumables, this.Status);
    }
}
