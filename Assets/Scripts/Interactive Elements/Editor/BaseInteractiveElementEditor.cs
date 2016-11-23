using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(BaseInteractiveElement))]
public class BaseInteractiveElementEditor : Editor {

    BaseInteractiveElement interactiveElement;

    protected virtual void OnEnable()
    {
        interactiveElement = target as BaseInteractiveElement;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawShapeSelector();
    }

    protected void DrawShapeSelector()
    {
        Collider2D collider2D = interactiveElement.GetComponent<Collider2D>();
        if (collider2D == null)
        {
            EditorGUILayout.HelpBox("A 2DCollider is needed!", MessageType.Error);
        }

        EditorGUILayout.LabelField("Select Shape");

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Circle")) { ReplaceBaseColliderType<CircleCollider2D>(); }
        if (GUILayout.Button("Box")) { ReplaceBaseColliderType<BoxCollider2D>(); }
        if (GUILayout.Button("Polygon")) { ReplaceBaseColliderType<PolygonCollider2D>(); }
        if (GUILayout.Button("Edge")) { ReplaceBaseColliderType<EdgeCollider2D>(); }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }

    void ReplaceBaseColliderType<T>() where T: Collider2D
    {
        Collider2D toRemove = interactiveElement.gameObject.GetComponentInChildren<Collider2D>();

        while (toRemove != null)
        {
            if (Application.isPlaying)
            {
                Object.Destroy(toRemove);
            }
            else
            {
                Object.DestroyImmediate(toRemove);
            }
            toRemove = interactiveElement.gameObject.GetComponentInChildren<Collider2D>();
        }
        
        // Add new Collider2D
       interactiveElement.gameObject.AddComponent<T>();
    }
}
