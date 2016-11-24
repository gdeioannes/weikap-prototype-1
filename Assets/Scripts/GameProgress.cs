using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameProgress : MonoBehaviour
{
    public static GameProgress Instance { get; private set; }

    public HashSet<int> SamplesCollected { get; private set; }    
    public System.Action OnSamplesCollectionUpdated = delegate { };

    void OnAwake()
    {
        Object.DontDestroyOnLoad(this.gameObject);        
    }

    public static void Instantiate()
    {
        GameObject newObject = new GameObject("GamerProgress");
        Instance = newObject.AddComponent<GameProgress>();
        Instance.SamplesCollected = new HashSet<int>();
    }

    public void UpdateSamplesCollected(int newSampleId)
    {
        SamplesCollected.Add(newSampleId);
        OnSamplesCollectionUpdated();
    }
}
