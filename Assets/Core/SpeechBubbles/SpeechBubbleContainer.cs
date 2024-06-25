using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleContainer : MonoBehaviour
{
    const float SpeechBubbleTimeOut = 4.5f;
    
    const float MaxVisibleScale = 3.1f;
    const float UnitsToObjectScale = 3.6f;
    public Transform target = null;
    public Vector3 offset = Vector3.zero;

    public float desiredViewportHeight = 0.1f;    

    private float currentTime = 0.0f;

    void LateUpdate()
    {
        if (target != null) {
            transform.position = target.position + offset;
        }

        var mainCamera = Camera.main;

        float distance = Vector3.Distance(mainCamera.transform.position, transform.position);
        float frustumHeight = 2.0f * distance * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);

        float desiredScreenHeightInWorldUnits = frustumHeight * desiredViewportHeight;

        float sc = desiredScreenHeightInWorldUnits * UnitsToObjectScale;
        if (sc > MaxVisibleScale) {
            sc = 0.0f;
        }
        transform.localScale = new Vector3(sc, sc, sc);

        transform.rotation = mainCamera.transform.rotation;
    }

    public void SetTarget(Transform target, Vector3 offset) {
        this.target = target;
        this.offset = offset;
    }

    public IEnumerator DisplayMessage(GameObject newSpeechBubble)
    {
        var speechBubbleBehaviour = newSpeechBubble.GetComponent<SpeechBubbleBehaviour>();
        var height = speechBubbleBehaviour.GetHeight();
        foreach (Transform child in transform) {
            var position = child.GetComponent<RectTransform>().localPosition;
            position.y += height;
            child.GetComponent<RectTransform>().localPosition = position;
        }
        newSpeechBubble.transform.SetParent(transform, false);
        float endTime = currentTime + SpeechBubbleTimeOut;
        while (currentTime < endTime) {
            yield return null;
        }
        Destroy(newSpeechBubble);
    }

    void Update() {
        currentTime += Time.deltaTime;
    }

    public void OnUserPressedAdvanceKey() {
        currentTime += SpeechBubbleTimeOut;
    }
}
