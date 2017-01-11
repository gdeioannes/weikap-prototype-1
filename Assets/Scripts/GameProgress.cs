using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameProgress : MonoBehaviour
{
    public static GameProgress Instance { get; private set; }

    public ulong CoinsCollected { get; private set; }
    public ulong CoinsAvailable { get; private set; }
    public HashSet<int> SamplesCollected { get; private set; }
    public HashSet<int> ToolsUnlocked { get; private set; }

    public System.Action OnSamplesCollectionUpdated = delegate { };
    public System.Action<ulong> OnCoinsAmountUpdated = delegate { };

    void OnAwake()
    {
        Object.DontDestroyOnLoad(this.gameObject);   
    }

    public static void Instantiate()
    {
        GameObject newObject = new GameObject("GamerProgress");
        Instance = newObject.AddComponent<GameProgress>();
        Instance.SamplesCollected = new HashSet<int>();
        Instance.ToolsUnlocked = new HashSet<int>();
    }

    public void UpdateSamplesCollected(int newSampleId)
    {
        SamplesCollected.Add(newSampleId);
        OnSamplesCollectionUpdated();
    }

    public void UpdateCoinsCollected(ulong coins)
    {
        CoinsAvailable += coins;
        CoinsCollected = coins > 0 ? CoinsCollected + coins : CoinsCollected;
        OnCoinsAmountUpdated(CoinsAvailable);
    }

    void OnDestroy()
    {
        Instance = null;
    }
}
