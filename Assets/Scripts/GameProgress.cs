using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameProgress : MonoBehaviour
{
    public static GameProgress Instance { get; private set; }
        
    public Dictionary<int, bool> samplesCollected;

    void OnAwake()
    {
        Object.DontDestroyOnLoad(this.gameObject);
    }

    public static void Instantiate()
    {
        GameObject newObject = new GameObject("GamerProgress");
        Instance = newObject.AddComponent<GameProgress>();
        Instance.samplesCollected = new Dictionary<int, bool>();
    }
}
