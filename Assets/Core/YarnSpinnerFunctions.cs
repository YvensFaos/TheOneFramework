using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public static class YarnSpinnerFunctions
{
    const float DefaultMinDistance = 2f;

    private static int currentAnimationValidationId = 0;

    static float GetCurrentTime(Animator animator) {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }


    private static GameObject FindGameObject(string name) {
        GameObject result = GameObject.Find(name);
        if (result == null) {
            Debug.LogError(string.Format("GameObject {0} can't be found", name));
        }
        return result;
    }

    private static T FindComponentOnObject<T>(string name) where T:Component {
        GameObject gameObject = FindGameObject(name);
        if (gameObject != null) {
            return gameObject.GetComponent<T>();
        }
        Debug.LogError(string.Format("GameObject {0} has no component of the type {1}", name, typeof(T).Name));
        return null;
    }


    [YarnCommand("play_animation")]
    public static IEnumerator PlayAnimation(string target, string name, bool shouldWait=true) {
        Animator animator = FindComponentOnObject<Animator>(target);
        if (animator != null) {
            animator.Play(name);
            if (shouldWait) {
                currentAnimationValidationId ++;
                int validationId = currentAnimationValidationId;
                while (GetCurrentTime(animator) >= 1.0f) {
                    if (currentAnimationValidationId != validationId) break;
                    yield return null;
                }
                while (GetCurrentTime(animator) < 1.0f) {
                    if (currentAnimationValidationId != validationId) break;
                    yield return null;
                }
            }
        }
    }

    [YarnCommand("set_enabled")]
    public static void SetEnabled(string target, bool mode=true) 
    {
        GameObject gameObject = FindGameObject(target);
        if (gameObject == null) return;
        foreach (PlayerActivatable playerActivatable in gameObject.GetComponents<PlayerActivatable>()) {
            playerActivatable.canBeActivated = mode;
        }
    }

    [YarnCommand("activate")]
    public static void Activate(string target) 
    {
        GameObject gameObject = FindGameObject(target);
        if (gameObject == null) return;
        foreach (PlayerActivatable playerActivatable in gameObject.GetComponents<PlayerActivatable>()) {
            playerActivatable.Activate();
        }
    }

    [YarnCommand("enable_controls")]
    public static void EnableControls(string target) 
    {
        var togglePlayerControls = FindComponentOnObject<TogglePlayerControls>(target);
        if (togglePlayerControls == null) return;
        togglePlayerControls.EnableControls();
    }

    [YarnCommand("disable_controls")]
    public static void DisableControls(string target) 
    {
        var togglePlayerControls = FindComponentOnObject<TogglePlayerControls>(target);
        if (togglePlayerControls == null) return;
        togglePlayerControls.DisableControls();
    }

    [YarnCommand("give_item")]
    public static void GiveItem(string target, string id) {
        var inventory = FindComponentOnObject<Inventory>(target);
        inventory.AddItem(id);
    }

    [YarnCommand("take_item")]
    public static void TakeItem(string target, string id) {
        var inventory = FindComponentOnObject<Inventory>(target);
        inventory.RemoveItem(id);
    }


    [YarnCommand("hide_path")]
    public static void HidePath() {
        var pathVisualizer = GameObject.FindObjectOfType<PathVisualizer>();
        if (pathVisualizer != null) {
            pathVisualizer.ClearPath();
        }
    }

    [YarnCommand("show_path")]
    public static System.Collections.IEnumerator ShowPath(string fromName, string toName, bool shouldWait=true, float minDistance=DefaultMinDistance) {
        var pathVisualizer = GameObject.FindObjectOfType<PathVisualizer>();
        if (pathVisualizer != null) {
            GameObject from = FindGameObject(fromName);
            GameObject to = FindGameObject(toName);
            if (from != null && to != null) {
                pathVisualizer.SetTargets(from.transform, to.transform);
                if (shouldWait) {
                    while (Vector3.Distance(from.transform.position, to.transform.position) > minDistance) {
                        yield return null;
                    }
                    pathVisualizer.ClearPath();
                }                
            }
        }
    }

    [YarnFunction("get_distance")]
    public static float GetDistanceByName(string from, string to) {
        GameObject object1 = FindGameObject(from);
        GameObject object2 = FindGameObject(to);
        return GetDistance(object1, object2);
    }

    private static float GetDistance(GameObject gameObject, GameObject other) {
        Vector3 delta = other.transform.position - gameObject.transform.position;
        return delta.magnitude;
    }

    [YarnCommand("wait_for_location")]
    public static IEnumerator WaitToReachTarget(GameObject gameObject, GameObject target, float treshold=1.5f) {
        float distance = 0f;
        do {
            distance = GetDistance(gameObject, target);
            yield return null;
        } while (distance > treshold);
    }

    [YarnCommand("set_spawn")]
    public static void SetRespawnPoint(string targetName, string spawnPointName)
    {
        var respawnBehaviour = FindComponentOnObject<RespawnBehaviour>(targetName);
        if (respawnBehaviour == null) return;
        GameObject spawnPoint = FindGameObject(spawnPointName);
        if (spawnPoint != null) {
            respawnBehaviour.SetRespawnPoint(spawnPoint);
        }
    }

    [YarnCommand("respawn")]
    public static void Respawn(string targetName)
    {
        var respawnBehaviour = FindComponentOnObject<RespawnBehaviour>(targetName);
        if (respawnBehaviour == null) return;
        respawnBehaviour.Respawn();
    }

    [YarnCommand("teleport")]
    public static void Teleport(string targetName, string locationName) {
        var respawnBehaviour = FindComponentOnObject<RespawnBehaviour>(targetName);
        if (respawnBehaviour == null) return;
        GameObject location = FindGameObject(locationName);
        if (location == null) return;
        respawnBehaviour.TeleportTo(location.transform.position);
    }

    [YarnCommand("say")]
    public static IEnumerator Say2(string targetName, string message, float offset_y = 0.0f, bool shouldWait=true) 
    {
        GameObject target = FindGameObject(targetName);
        if (target == null) { 
            return null; 
        } else {
            Vector3 offset = new Vector3(0f, offset_y, 0f);
            return GameObject.FindObjectOfType<SpeechBubbleManager>().AddSpeechBubble(target, offset, message, shouldWait);
        }
    }

    [YarnCommand("toggle_animation")]
    public static void ToggleAnimationStates(string targetName) {
        AnimationToggle animationToggle = FindComponentOnObject<AnimationToggle>(targetName);
        if (animationToggle != null) {
            animationToggle.ToggleAnimationStates();
        }
    }

    [YarnCommand("animate")]
    public static void Animate(string targetName) {
        SimpleAnimationTrigger simpleAnimationTrigger = FindComponentOnObject<SimpleAnimationTrigger>(targetName);
        if (simpleAnimationTrigger != null) {
            simpleAnimationTrigger.Animate();
        }
    }

    [YarnCommand("walk_to")]
    public static IEnumerator WalkTo(string targetName, string destinationName, float distance=0f, bool shouldWait = true) {
        WalkToBehaviour target = FindComponentOnObject<WalkToBehaviour>(targetName);
        if (target != null) {
            GameObject destination = FindGameObject(destinationName);
            return target.WalkTo(destination, distance, shouldWait);
        } else {
            return null;
        }
    }

}
