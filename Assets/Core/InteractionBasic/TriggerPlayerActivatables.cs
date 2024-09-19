using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Rigidbody))]
public class TriggerPlayerActivatables : MonoBehaviour
{
    private HashSet<PlayerActivatable> _previousHits = new HashSet<PlayerActivatable>();
    private HashSet<PlayerActivatable> _currentHits = new HashSet<PlayerActivatable>();
    
    void Awake() {
        // Ensure a Rigidbody component is present and set it to kinematic
        Rigidbody rb = GetComponent<Rigidbody>();
        if (!rb.isKinematic) return;
        Debug.LogWarning($"{gameObject.name} Trigger Player Activatables is using a non-kinematic rigidbody. It will be forcibly set to kinematic.");
        rb.isKinematic = true;
    }

    void Update()
    {
        if (_currentHits.Count == 0) return;
        foreach (PlayerActivatable playerActivatable in _currentHits) {
            if (!_previousHits.Contains(playerActivatable)) {
                playerActivatable.OnColliding();
            }
        }

        // Swap the sets to avoid reallocating memory
        _previousHits.Clear();
        var temp = _previousHits;
        _previousHits = _currentHits;
        _currentHits = temp;
        _currentHits.Clear();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        IterateOverPlayerActivatables(hit.gameObject.GetComponents<PlayerActivatable>());
    }

    private void IterateOverPlayerActivatables(PlayerActivatable[] playerActivatables)
    {
        if (playerActivatables == null) return;
        foreach (PlayerActivatable playerActivatable in playerActivatables) {
            if (!_previousHits.Contains(playerActivatable)) {
                playerActivatable.OnColliding();
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        HandleCollision(other.gameObject);
    }

    void OnTriggerStay(Collider other) {
        HandleCollision(other.gameObject);
    }

    void OnTriggerExit(Collider other) {
        SolveExitState(other.gameObject);
    }

    private void SolveExitState(GameObject exitObject)
    {
        PlayerActivatable[] playerActivatables = exitObject.GetComponents<PlayerActivatable>();
        if (playerActivatables == null) return;
        foreach (PlayerActivatable playerActivatable in playerActivatables)
        {
            if (playerActivatable == null) continue;
            _currentHits.Remove(playerActivatable);
            _previousHits.Remove(playerActivatable);
        }
    }

    private void HandleCollision(GameObject obj, bool addToList = true) {
        PlayerActivatable[] playerActivatables = obj.GetComponents<PlayerActivatable>();
        if (playerActivatables == null) return;
        foreach (PlayerActivatable playerActivatable in playerActivatables) {
            if (playerActivatable != null && playerActivatable.canBeActivated) {
                _currentHits.Add(playerActivatable);
            }
        }
    }
}
