using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TriggerPlayerActivatables : MonoBehaviour
{
    private HashSet<PlayerActivatable> _previousHits = new HashSet<PlayerActivatable>();
    private HashSet<PlayerActivatable> _currentHits = new HashSet<PlayerActivatable>();
    
    void Awake() {
        // Ensure a Rigidbody component is present and set it to kinematic
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) {
            rb = gameObject.AddComponent<Rigidbody>();
        }
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

    void OnControllerColliderHit(ControllerColliderHit hit) {
        HandleCollision(hit.gameObject);
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

    private void HandleCollision(GameObject obj) {
        PlayerActivatable[] playerActivatables = obj.GetComponents<PlayerActivatable>();
        if (playerActivatables == null) return;
        foreach (PlayerActivatable playerActivatable in playerActivatables) {
            if (playerActivatable != null && playerActivatable.canBeActivated) {
                _currentHits.Add(playerActivatable);
            }
        }
    }
}
