#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class ShowObjectNamesEditor : MonoBehaviour
{
    [System.Obsolete]
    void OnDrawGizmos()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.alignment = TextAnchor.MiddleCenter;

            foreach (var renderer in FindObjectsOfType<Renderer>())
            {
                if (renderer.gameObject.layer == 0) {
                    if (renderer.gameObject.GetComponent<PlayerActivatable>()) {
                        Vector3 labelPosition = renderer.transform.position + new Vector3(0f, renderer.bounds.extents.y, 0f);
                        Handles.Label(labelPosition, renderer.gameObject.name, style);
                    }
                }
            }
        }
    }
}
#endif
