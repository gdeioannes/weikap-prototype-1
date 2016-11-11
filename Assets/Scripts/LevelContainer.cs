﻿using UnityEngine;
using System.Collections;

public class LevelContainer : MonoBehaviour
{
    [System.Serializable]
    public struct ConsumableConfig
    {
        public ConsumableController.ConsumableType type;
        public GameObject prefab;
    }

    const string LIMIT_ZONES_CONTAINER_NAME = "LimitZonesContainer";
    const string WIND_ZONES_CONTAINER_NAME = "WindZonesContainer";
    const string DAMAGE_ZONES_CONTAINER_NAME = "DamageZonesContainer";    
    const string CONSUMABLE_ZONE_CONTAINER_NAME = "ConsumablesContainer";
    const string CONSUMABLE_TYPE_CONTAINER_NAME = "ConsumableType";
    const string LIMIT_ZONES_CHILD = "LimitZone";
    const string WIND_ZONE_CHILD = "WindZone";
    const string DAMAGE_ZONE_CHILD = "DamageZone";
    const string CONSUMABLE_ZONE_CHILD = "Consumable";

    Transform cacheTransform;

    public Vector2 Size;
    [Header("Level elements config")]
    public ConsumableConfig[] consumablesConfig;
    public GameObject damageZonePrefab;
    public GameObject windZonePrefab;

    public Vector2 CenterPosition
    {
        get
        {
            if (cacheTransform == null) { cacheTransform = this.transform; }
            Vector2 toReturn = cacheTransform.position - Vector3.left * (Size.x * 0.5f);
            toReturn += Vector2.up * (Size.y * 0.5f);
            return toReturn;
        }
    }    

    [ContextMenu("Create Limit Zones")]
    void CreateLimitZones()
    {
        Transform limitZonesContainer = this.transform.FindChild(LIMIT_ZONES_CONTAINER_NAME);
        if (limitZonesContainer != null) // detach and destroy all children
        {
            while (limitZonesContainer.childCount > 0)
            {
                Transform firstChild = limitZonesContainer.GetChild(0);
                firstChild.SetParent(null, true);
                Object.DestroyImmediate(firstChild.gameObject);
            }
        }
        else
        {
            GameObject limitZonesGo = new GameObject(LIMIT_ZONES_CONTAINER_NAME);
            limitZonesGo.transform.SetParent(this.transform, false);
            limitZonesContainer = limitZonesGo.transform;            
        }

        // Create zones
        GameObject limitZoneBottomLeft = new GameObject(string.Format("{0}_BottomLeft", LIMIT_ZONES_CHILD));
        GameObject limitZoneTopLeft = new GameObject(string.Format("{0}_TopLeft", LIMIT_ZONES_CHILD));        
        GameObject limitZoneBottomRight = new GameObject(string.Format("{0}_BottomRight", LIMIT_ZONES_CHILD));

        limitZoneBottomLeft.transform.SetParent(limitZonesContainer, false);
        limitZoneTopLeft.transform.SetParent(limitZonesContainer, false);        
        limitZoneBottomRight.transform.SetParent(limitZonesContainer, false);

        // Set Positions
        limitZoneTopLeft.transform.localPosition = Vector3.up * Size.y;        
        limitZoneBottomRight.transform.localPosition = Vector3.right * Size.x;

        // Add Colliders
        BoxCollider2D collider = limitZoneBottomLeft.AddComponent<BoxCollider2D>();
        collider.size = Vector2.one + Vector2.right * Size.x;
        collider.offset = Vector2.right * (Size.x * 0.5f) + (Vector2.down * 0.5f);

        collider = limitZoneTopLeft.AddComponent<BoxCollider2D>();
        collider.size = Vector2.one + Vector2.right * Size.x;
        collider.offset = Vector2.right * (Size.x * 0.5f) + (Vector2.up * 0.5f);

        collider = limitZoneBottomLeft.AddComponent<BoxCollider2D>();
        collider.size = Vector2.one + Vector2.up * Size.y;
        collider.offset = Vector2.up * (Size.y * 0.5f) + (Vector2.left * 0.5f);

        collider = limitZoneBottomRight.AddComponent<BoxCollider2D>();
        collider.size = Vector2.one + Vector2.up * Size.y;
        collider.offset = Vector2.up * (Size.y * 0.5f) + (Vector2.right * 0.5f);
    }

    [ContextMenu("Create Wind Zone")]
    void CreateWindZone()
    {
        Transform parent = CreateContainer(WIND_ZONES_CONTAINER_NAME);
        string childName = string.Format("{0}_{1}", WIND_ZONE_CHILD, parent.childCount);
        GameObject windZoneGo = Object.Instantiate<GameObject>(windZonePrefab);
        windZoneGo.name = childName;

        #if UNITY_EDITOR
        UnityEditor.Selection.activeGameObject = windZoneGo;
        #endif
    }

    [ContextMenu("Create Damage Zone")]
    void CreateDamageZone()
    {
        Transform parent = CreateContainer(DAMAGE_ZONES_CONTAINER_NAME);
        string childName = string.Format("{0}_{1}", DAMAGE_ZONE_CHILD, parent.childCount);
        GameObject damageZoneGo = Object.Instantiate<GameObject>(damageZonePrefab);
        damageZoneGo.name = childName;                

        #if UNITY_EDITOR
        UnityEditor.Selection.activeGameObject = damageZoneGo;
        #endif
    }

    [ContextMenu("Create Coin Consumable")]
    void CreateCoinConsumable()
    {
        GameObject newConsumable = CreateConsumableObject(ConsumableController.ConsumableType.Coin);
        
        #if UNITY_EDITOR
        UnityEditor.Selection.activeGameObject = newConsumable;
        #endif
    }

    [ContextMenu("Create Sample Consumable")]
    void CreateSampleConsumable()
    {
        GameObject newConsumable = CreateConsumableObject(ConsumableController.ConsumableType.Sample);

        #if UNITY_EDITOR
        UnityEditor.Selection.activeGameObject = newConsumable;
        #endif        
    }    

    GameObject CreateConsumableObject(ConsumableController.ConsumableType type)
    {
        string containerName = string.Format("{0}/{1}_{2}", CONSUMABLE_ZONE_CONTAINER_NAME, CONSUMABLE_TYPE_CONTAINER_NAME, type.ToString());

        Transform parent = CreateContainer(containerName);
        string childName = string.Format("{0}_{1}", CONSUMABLE_ZONE_CHILD, parent.childCount);
        GameObject childGo = null;

        foreach (var prefab in consumablesConfig)
        {
            if (prefab.type == type)
            {
                childGo = Object.Instantiate<GameObject>(prefab.prefab);
                break;
            }
        }
        if (childGo == null)
        {
            childGo = new GameObject();
            ConsumableController consumableController = childGo.AddComponent<ConsumableController>();
            consumableController.type = type;
        }

        childGo.name = childName;
        childGo.transform.SetParent(parent, false);
        return childGo;
    }    

    Transform CreateContainer(string containerName)
    {
        Transform parent = this.transform;
        Transform childContainer = null;
        string[] paths = containerName.Split('/');
        foreach (var path in paths)
        {
            childContainer = parent.FindChild(path);
            if (childContainer == null) // Create Child
            {
                GameObject newParent = new GameObject(path);
                newParent.transform.SetParent(parent, false);
                childContainer = newParent.transform;
            }
            parent = childContainer;
        }

        return parent;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(CenterPosition, Size);
    }
}
