using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelProgress : MonoBehaviour {    

    public Dictionary<InGameItemsDBScriptableObject.ItemType, int> consumables;    
	public HashSet<int> samples;
    public int Id { get; private set; }
	public PlayerData.LevelStatus Status { get; private set; }

    void Awake()
    {
        consumables = new Dictionary<InGameItemsDBScriptableObject.ItemType, int>();
		samples = new HashSet<int>();
		this.Status = PlayerData.LevelStatus.OnGoing;
    }

	public void UpdateGameProgress(PlayerData.LevelStatus status)
    {
        this.Status = status;
		PlayerData.Instance.UpdateLevelProgress(Id, consumables, this.Status);
    }
}
