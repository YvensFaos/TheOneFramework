using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityTrigger : MonoBehaviour
{
    public float longPressDuration = 2f;

    private Material material;
    private float progress = 0f;
    private bool currentActivationMode = false;
    private bool isCurrentlyVisible = false;
    private bool isCurrentlyPressed = false;

    private SpriteRenderer spriteRenderer;
    private bool isActivated = false;

    private Transform originalParent = null;

    public GameObject targetPlayerActivatable;

    public Vector3 offset = Vector3.zero;

    private Transform proximityRoot = null;

    [System.Obsolete]
    void Start()
    {
        proximityRoot = FindFirstObjectByType<ProximityTriggerRoot>().transform;
        if (proximityRoot == null) {
            Debug.LogWarning("No ProximityTriggerRoot found in scene. ProximityTrigger will be deactivated");
            enabled = false;
            Destroy(gameObject);
            return;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
        spriteRenderer.enabled = false;

        if (targetPlayerActivatable == null) {
            targetPlayerActivatable = transform.parent.gameObject;
        }

        originalParent = transform.parent;
        transform.SetParent(proximityRoot.transform);
        transform.localScale = Vector3.one;
    }

    public void SetVisibilityMode(bool mode) {
        isCurrentlyVisible = mode;
        spriteRenderer.enabled = mode;
    }

    public void SetUserInteractionState(bool mode) {
        isCurrentlyPressed = mode;
    }

    void Update()
    {
        bool mode = isCurrentlyPressed && isCurrentlyVisible;
        if (currentActivationMode != mode) {
            currentActivationMode = mode;
            progress = 0f;
            isActivated = false;
        }
        if (currentActivationMode) {
            if (!isActivated) {
                progress += Time.deltaTime * 100f / longPressDuration;
                if (progress >= 100f) {
                    isActivated = true;
                    HandleActivation();
                }
                progress = Mathf.Clamp(progress, 0f, 100f);
            }
        }
        material.SetFloat("_Progress", progress);
    }

    public bool IsAvailable() {
        if (targetPlayerActivatable) {
            foreach (PlayerActivatable playerActivatable in targetPlayerActivatable.GetComponents<PlayerActivatable>()) {
                if (playerActivatable.IsAvailable()) {
                    return true;
                }
            }
            return false;
        } 
        return false;
    }

    void HandleActivation() 
    {
        if (targetPlayerActivatable) {
            SetVisibilityMode(false);
            foreach (PlayerActivatable playerActivatable in targetPlayerActivatable.GetComponents<PlayerActivatable>()) {
                playerActivatable.Activate();
            }
        }
    }


    void LateUpdate()
    {
        if (originalParent != null) {
            transform.position = originalParent.position + offset;
        }
        transform.rotation = Camera.main.transform.rotation;    
    }
}
