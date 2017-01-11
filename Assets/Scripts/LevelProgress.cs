using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelProgress : MonoBehaviour {    

    public Dictionary<InGameItemsDBScriptableObject.ItemType, int> consumables;    
    public int Id { get; private set; }
    GameProgress.LevelStatus status;

    void Awake()
    {
        consumables = new Dictionary<InGameItemsDBScriptableObject.ItemType, int>();
        this.status = GameProgress.LevelStatus.OnGoing;
    }

    public void UpdateGameProgress(GameProgress.LevelStatus status)
    {
        this.status = status;
        GameProgress.Instance.UpdateLevelProgress(Id, consumables, this.status);
    }
}
