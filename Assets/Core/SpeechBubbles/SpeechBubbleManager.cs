using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class SpeechBubbleManager : MonoBehaviour
{
    public GameObject speechBubblePrefab;

    public Dictionary<GameObject, SpeechBubbleContainer> speechBubbleContainers = new Dictionary<GameObject, SpeechBubbleContainer>();
    public GameObject target;

    public IEnumerator CreateMessage(SpeechBubbleContainer container, string message) {
        GameObject newSpeechBubble = GameObject.Instantiate(speechBubblePrefab);
        var speechBubbleBehaviour = newSpeechBubble.GetComponent<SpeechBubbleBehaviour>();
        speechBubbleBehaviour.SetMessage(message);
        return container.DisplayMessage(newSpeechBubble);
    }


    public SpeechBubbleContainer GetSpeechBubbleContainer(GameObject target, Vector3 offset) {
        if (target == null) {
            Debug.LogError("Speech bubble target is null. Probably the object name is not found.");
        }
        if (speechBubbleContainers.ContainsKey(target)) {
            SpeechBubbleContainer speechBubbleContainer = speechBubbleContainers[target];
            speechBubbleContainer.SetTarget(target.transform, offset);
            return speechBubbleContainer;
        } else {
            GameObject newContainerGameObject = new GameObject("SpeechBubbleContainer");
            newContainerGameObject.transform.SetParent(transform, false);
            SpeechBubbleContainer speechBubbleContainer = newContainerGameObject.AddComponent<SpeechBubbleContainer>();
            speechBubbleContainer.SetTarget(target.transform, offset);
            speechBubbleContainers[target] = speechBubbleContainer;

            var speechBubbleKeypressAdvance = GetComponent<SpeechBubbleKeypressAdvance>();
            if (speechBubbleKeypressAdvance != null) {
                speechBubbleKeypressAdvance.OnUserPressedAdvance += speechBubbleContainer.OnUserPressedAdvanceKey;
            }
            return speechBubbleContainer;
        }
    }

    public IEnumerator AddSpeechBubble(GameObject target, Vector3 offset, string message, bool shouldWait) {
        SpeechBubbleContainer speechBubbleContainer = GetSpeechBubbleContainer(target, offset);
        if (shouldWait) {
            return CreateMessage(speechBubbleContainer, message);
        } else {
            StartCoroutine(CreateMessage(speechBubbleContainer, message));
            return null;            
        }
    }

}
