using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
        // get current level id
        var currentLevelInfo = PlayerData.Instance.Levels.Where(w => w.Value.scene.name == UnityEngine.SceneManagement.SceneManager.GetSceneAt(0).name).FirstOrDefault();
        this.Id = currentLevelInfo.Key;
    }

	public void UpdateGameProgress(PlayerData.LevelStatus status)
    {
        this.Status = status;
		PlayerData.Instance.UpdateLevelProgress(Id, consumables, this.Status);
    }

    public void SaveCollectedItemsInLevel()
    {
        foreach (var item in consumables)
        {
            if (item.Key == InGameItemsDBScriptableObject.ItemType.Coin)
            {
                PlayerData.Instance.UpdateCoinsCollected((long)item.Value);
            }
        }

        foreach (var sampleId in samples)
        {
            PlayerData.Instance.UpdateSamplesCollected(sampleId);
        }

        PlayerData.Instance.UpdateMaxValues(Id, consumables);        
    }
}
