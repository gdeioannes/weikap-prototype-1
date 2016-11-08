using UnityEngine;
using System.Collections;

public class LevelContainer : MonoBehaviour
{
    const string LIMIT_ZONES_CONTAINER_NAME = "LimitZonesContainer";
    const string LIMIT_ZONES_CHILD = "LimitZone";
    Transform cacheTransform;

    public Vector2 Size;

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

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(CenterPosition, Size);
    }
}
