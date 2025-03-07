using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Yarn.Unity;

public class PathVisualizer : MonoBehaviour
{
    const float DefaultMinDistance = 2f;
    const float MinPathLength = 2f;
    const float DecalSpacing = 1f;
        
    public GameObject decalPrefab;
    private List<GameObject> decals = new List<GameObject>();

    private Transform source = null;
    private Transform target = null;

    private NavMeshPath path;

    private bool showPath = false;

    void Start() {
        path = new NavMeshPath();
    }

    void Update()
    {
        DrawPath();
    }

    public void SetTargets(Transform from, Transform to)
    {
        source = from;
        target = to;
        showPath = (source != null) && (target != null);
    }

    private void ClearDecals()
    {
        if (decals.Count > 0) {
            foreach (var decal in decals)
            {
                Destroy(decal);
            }
            decals.Clear();
        }
    }   

    private void SpawnDecals(Vector3[] pathCorners) {
        Vector3 previousPoint = pathCorners[0];
        for (int i = 1; i < pathCorners.Length; i++) {

            Vector3 currentPoint = pathCorners[i];
            float distance = Vector3.Distance(previousPoint, currentPoint);
            Vector3 direction = (currentPoint - previousPoint).normalized;

            for (float j = 0; j < distance; j += DecalSpacing) {
                Vector3 position = Vector3.Lerp(previousPoint, currentPoint, j / distance);
                Quaternion rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -90, 0);
                GameObject decal = Instantiate(decalPrefab, position, rotation);
                decal.transform.SetParent(transform);
                decals.Add(decal);

                var projector = decal.transform.GetChild(0).GetComponent<UnityEngine.Rendering.Universal.DecalProjector>();
                projector.uvBias = new Vector3(-Time.time * 2f, 0f);
            }

            previousPoint = currentPoint;
        }        
    } 

    public void ClearPath() {
        showPath = false;
    }

    [System.Obsolete]
    public static void HidePath() {
        var pathVisualizer = FindFirstObjectByType<PathVisualizer>();
        if (pathVisualizer != null) {
            pathVisualizer.ClearPath();
        }
    }

    [System.Obsolete]
    public static System.Collections.IEnumerator ShowPath(GameObject from, GameObject to, bool shouldWait=true, float minDistance=DefaultMinDistance) {
        var pathVisualizer = FindFirstObjectByType<PathVisualizer>();
        if (pathVisualizer != null) {
            pathVisualizer.SetTargets(from.transform, to.transform);
            if (shouldWait) {
                while (Vector3.Distance(from.transform.position, to.transform.position) > minDistance) {
                    yield return null;
                }
            }
            pathVisualizer.ClearPath();
        }
    }

    public void DrawPath() 
    {
        ClearDecals();
        if (!showPath) return;
        if (source == null || target == null) return;

        Vector3 inverseDirection = (target.position - source.position).normalized;
        Vector3 pos = target.position - inverseDirection;
        NavMesh.CalculatePath(source.position, pos, NavMesh.AllAreas, path);
        Vector3[] pathCorners = path.corners;
        if (pathCorners.Length < 2) return;

        float totalPathLength = 0f;
        for (int i = 1; i < pathCorners.Length; i++) {
            totalPathLength += Vector3.Distance(pathCorners[i - 1], pathCorners[i]);
        }
        if (totalPathLength < MinPathLength) return;

        SpawnDecals(pathCorners);
    }    
}
