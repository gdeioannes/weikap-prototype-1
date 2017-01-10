using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInGameItemsDB", menuName = "Weikap/DB/InGameItems")]
public class InGameItemsDBScriptableObject : ScriptableObject, ISerializationCallbackReceiver
{
    public enum ItemType
    {
        Coin,
        Sample,
        Question
    }

    [System.Serializable]
    public class ItemConfig
    {
        public ItemType ItemType;
        public GameObject prefab;
    }

    [SerializeField] ItemConfig[] items;
    public Dictionary<ItemType, ItemConfig> Items { get; private set; }

    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
        if (Items == null) 
        {
            Items = new Dictionary<ItemType, ItemConfig>();
        }
        Items.Clear();

        foreach (var item in items)
        {
            Items[item.ItemType] = item;
        }
    }
}
