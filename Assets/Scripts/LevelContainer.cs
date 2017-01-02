using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelContainer : MonoBehaviour
{
    [System.Serializable]
    public struct ConsumableConfig
    {
        public ConsumableController.ConsumableType type;
        public GameObject prefab;
    }

    const string LIMIT_ZONES_CONTAINER_NAME = "LimitZonesContainer";
    const string SPAWN_POINTS_CONTAINER_NAME = "SpawnPoints";
    const string WIND_ZONES_CONTAINER_NAME = "WindZonesContainer";
    const string VOLCANO_ZONES_CONTAINER_NAME = "VolcanoZonesContainer";
    const string GEYSER_ZONES_CONTAINER_NAME = "GeyserZonesContainer";
    const string SURFACE_ZONES_CONTAINER_NAME = "SurfaceZonesContainer";
    const string DAMAGE_ZONES_CONTAINER_NAME = "DamageZonesContainer";
    const string CONSUMABLE_ZONE_CONTAINER_NAME = "ConsumablesContainer";
    const string QUESTIONS_CONTAINER_NAME = "QuestionsContainer";

    const string CONSUMABLE_TYPE_CONTAINER_NAME = "ConsumableType";

    const string LIMIT_ZONES_CHILD = "LimitZone";
    const string SPAWN_POINT_CHILD = "SpawnPoint";
    const string WIND_ZONE_CHILD = "WindZone";
    const string VOLCANO_ZONE_CHILD = "VolcanoZone";
    const string GEYSER_ZONE_CHILD = "GeyserZone";
    const string SURFACE_ZONE_CHILD = "SurfaceZone";
    const string DAMAGE_ZONE_CHILD = "DamageZone";
    const string CONSUMABLE_ZONE_CHILD = "Consumable";
    const string QUESTION_CHILD = "Question";    

    Transform cacheTransform;

    public Vector2 Size;
    [Header("Level elements config")]
    public SpawnPoint[] spawnPoints;
    public EndPoint endPoint;
    public ConsumableConfig[] consumablesConfig;
    public GameObject damageZonePrefab;
    public GameObject windZonePrefab;
    public GameObject volcanoZonePrefab;
    public GameObject geyserZonePrefab;
    public GameObject questionPrefab;
    public GameObject spawnPointPrefab;

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

    public int CoinsCount { get; private set; }
    public int SamplesCount { get; private set; }
    public int QuestionsCount { get; private set; }
    public List<int> samplesInLevel { get; private set; }

    void Awake()
    {
        // find all spawn points
        Transform parentContainer = this.CreateContainer(SPAWN_POINTS_CONTAINER_NAME);
        spawnPoints = parentContainer.GetComponentsInChildren<SpawnPoint>();
        parentContainer = this.CreateContainer(string.Format("{0}/{1}_{2}",CONSUMABLE_ZONE_CONTAINER_NAME, CONSUMABLE_TYPE_CONTAINER_NAME, ConsumableController.ConsumableType.Coin));
        CoinsCount = parentContainer.childCount;
        parentContainer = this.CreateContainer(string.Format("{0}/{1}_{2}", CONSUMABLE_ZONE_CONTAINER_NAME, CONSUMABLE_TYPE_CONTAINER_NAME, ConsumableController.ConsumableType.Sample));
        SamplesCount = parentContainer.childCount;

        samplesInLevel = new List<int>();
        ConsumableController[] sampleControllers = parentContainer.GetComponentsInChildren<ConsumableController>();
        foreach (var item in sampleControllers)
        {
            if (item.type == ConsumableController.ConsumableType.Sample)
            {
                samplesInLevel.Add(item.id);
            }
        }

        parentContainer = this.CreateContainer(QUESTIONS_CONTAINER_NAME);
        QuestionsCount = parentContainer.childCount;
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

    [ContextMenu("Create Spawn Point")]
    void CreateSpawnPoint()
    {
        Transform parent = CreateContainer(SPAWN_POINTS_CONTAINER_NAME);
        string childName = string.Format("{0}_{1}", SPAWN_POINT_CHILD, parent.childCount);
        GameObject newSpawnPoint = Object.Instantiate<GameObject>(spawnPointPrefab);
        newSpawnPoint.name = childName;
        newSpawnPoint.transform.SetParent(parent, false);

        #if UNITY_EDITOR
        UnityEditor.Selection.activeGameObject = newSpawnPoint;
        #endif
    }

    [ContextMenu("Create Wind Zone")]
    void CreateWindZone()
    {
        Transform parent = CreateContainer(WIND_ZONES_CONTAINER_NAME);
        string childName = string.Format("{0}_{1}", WIND_ZONE_CHILD, parent.childCount);
        GameObject windZoneGo = Object.Instantiate<GameObject>(windZonePrefab);
        windZoneGo.name = childName;
        windZoneGo.transform.SetParent(parent, false);

        #if UNITY_EDITOR
        UnityEditor.Selection.activeGameObject = windZoneGo;
        #endif
    }

    [ContextMenu("Create Volcano Zone")]
    void CreateVolcanoZone()
    {
        Transform parent = CreateContainer(VOLCANO_ZONES_CONTAINER_NAME);
        string childName = string.Format("{0}_{1}", VOLCANO_ZONE_CHILD, parent.childCount);
        GameObject zoneGo = Object.Instantiate<GameObject>(volcanoZonePrefab);
        zoneGo.name = childName;
        zoneGo.transform.SetParent(parent, false);

        #if UNITY_EDITOR
        UnityEditor.Selection.activeGameObject = zoneGo;     
        #endif
    }

    [ContextMenu("Create Geyser Zone")]
    void CreateGeyserZone()
    {
        Transform parent = CreateContainer(GEYSER_ZONES_CONTAINER_NAME);
        string childName = string.Format("{0}_{1}", GEYSER_ZONE_CHILD, parent.childCount);
        GameObject zoneGo = Object.Instantiate<GameObject>(geyserZonePrefab);
        zoneGo.name = childName;
        zoneGo.transform.SetParent(parent, false);

        #if UNITY_EDITOR
        UnityEditor.Selection.activeGameObject = zoneGo;
        #endif
    }

    [ContextMenu("Create Damage Zone")]
    void CreateDamageZone()
    {
        Transform parent = CreateContainer(DAMAGE_ZONES_CONTAINER_NAME);
        string childName = string.Format("{0}_{1}", DAMAGE_ZONE_CHILD, parent.childCount);
        GameObject damageZoneGo = Object.Instantiate<GameObject>(damageZonePrefab);
        damageZoneGo.name = childName;
        damageZoneGo.transform.SetParent(parent, false);

        #if UNITY_EDITOR
        UnityEditor.Selection.activeGameObject = damageZoneGo;
        #endif
    }

    [ContextMenu("Create Surface Zone")]
    void CreateSurfaceZone()
    {
        Transform parent = CreateContainer(SURFACE_ZONES_CONTAINER_NAME);
        string childName = string.Format("{0}_{1}", SURFACE_ZONE_CHILD, parent.childCount);
        GameObject surfaceZoneGo = new GameObject(childName);
        surfaceZoneGo.transform.SetParent(parent, false);
        BoxCollider2D collider = surfaceZoneGo.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        surfaceZoneGo.AddComponent<SurfaceZone>();

        #if UNITY_EDITOR
        UnityEditor.Selection.activeGameObject = surfaceZoneGo;
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

    [ContextMenu("Create Question Object")]
    void CreateQuestionObject()
    {
        Transform parent = CreateContainer(QUESTIONS_CONTAINER_NAME);
        string childName = string.Format("{0}_{1}", QUESTION_CHILD, parent.childCount);
        GameObject questionGo = Object.Instantiate<GameObject>(questionPrefab);
        questionGo.name = childName;
        questionGo.transform.SetParent(parent, false);
        QuestionZoneController questionController = questionGo.GetComponent<QuestionZoneController>();
        questionController.coinControllerPrefab = GetConsumablePrefabReference(ConsumableController.ConsumableType.Coin).GetComponent<ConsumableController>();

        #if UNITY_EDITOR
        UnityEditor.Selection.activeGameObject = questionGo;
        #endif
    }

    GameObject CreateConsumableObject(ConsumableController.ConsumableType type)
    {
        string containerName = string.Format("{0}/{1}_{2}", CONSUMABLE_ZONE_CONTAINER_NAME, CONSUMABLE_TYPE_CONTAINER_NAME, type.ToString());

        Transform parent = CreateContainer(containerName);
        string childName = string.Format("{0}_{1}", CONSUMABLE_ZONE_CHILD, parent.childCount);
        GameObject childGo = null;

        GameObject prefabReference = GetConsumablePrefabReference(type);

        if (prefabReference != null)
        {
            childGo = Object.Instantiate<GameObject>(prefabReference);
        }
        else
        {
            childGo = new GameObject();
            ConsumableController consumableController = childGo.AddComponent<ConsumableController>();
            consumableController.type = type;
            
        }

        childGo.name = childName;
        childGo.transform.SetParent(parent, false);
        return childGo;
    }

    GameObject GetConsumablePrefabReference(ConsumableController.ConsumableType type)
    {
        foreach (var prefab in consumablesConfig)
        {
            if (prefab.type == type)
            {
                return prefab.prefab;
            }
        }
        return null;
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
