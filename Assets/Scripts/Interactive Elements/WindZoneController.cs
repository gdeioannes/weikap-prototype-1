using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D), typeof(AreaEffector2D))]
[ExecuteInEditMode]
public class WindZoneController : MonoBehaviour
{
    BoxCollider2D boxCollider2D;
    AreaEffector2D areaEffector2D;

    [SerializeField] [Range(0,359)] float forceAngle;
    [SerializeField] [Range(0,15)] float forceMagnitude;
    [SerializeField] Vector2 size;

    void OnEnable()
    {
        boxCollider2D = this.GetComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = true;
        boxCollider2D.usedByEffector = true;
        areaEffector2D = this.GetComponent<AreaEffector2D>();

        UpdateComponents();
    }

    #if UNITY_EDITOR
    void Update()
    {
        UpdateComponents();
    }
#endif

    void UpdateComponents()
    {
        this.transform.localRotation = Quaternion.Euler(Vector3.back * forceAngle);
        areaEffector2D.forceMagnitude = forceMagnitude;

        boxCollider2D.size = size;
        boxCollider2D.offset = Vector2.right * (size.x * 0.5f);
    }

    void OnDrawGizmos()
    {
        if (boxCollider2D == null || areaEffector2D == null) { OnEnable(); }

        ForGizmo(this.transform.position, DegreeToVector2(this.transform.localRotation.eulerAngles.z), Color.blue, forceMagnitude, 0.25f,40f);
    }    

    public static void ForGizmo(Vector3 pos, Vector3 direction, Color color, float distance,  float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Gizmos.color = color;
        Gizmos.DrawRay(pos, direction * distance);

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0 - arrowHeadAngle, 0, 0) * Vector3.up;
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(180 + arrowHeadAngle, 0, 0) * Vector3.up;
        Gizmos.DrawRay(pos + direction * distance, right * arrowHeadLength);
        Gizmos.DrawRay(pos + direction * distance, left * arrowHeadLength);
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
}
