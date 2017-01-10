using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelProgress : MonoBehaviour {

    public Dictionary<InGameItemsDBScriptableObject.ItemType, int> consumables;
    public int answeredQuestions = 0;

    void Awake()
    {
        consumables = new Dictionary<InGameItemsDBScriptableObject.ItemType, int>();
    }
}
