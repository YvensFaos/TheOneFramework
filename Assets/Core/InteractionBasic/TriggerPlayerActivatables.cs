using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TriggerPlayerActivatables : MonoBehaviour
{
    private HashSet<Collider> _previousHits = new HashSet<Collider>();
    private HashSet<Collider> _currentHits = new HashSet<Collider>();

    private CharacterController characterController;

    void Awake() {
        characterController = GetComponent<CharacterController>();
    }

    void Update() {
        QueryTriggers();
    }

    void QueryTriggers() {
        float radius = characterController.radius;
        float capsuleHeight = characterController.height;

         // Calculate the top and bottom points of the capsule (relative to the current position)
        Vector3 capsuleCenter = transform.position + characterController.center;
        
        // Adjust for the character controller's height and radius
        float halfHeight = (capsuleHeight / 2) - radius;
        
        // Top and bottom points of the capsule
        Vector3 point1 = capsuleCenter + Vector3.up * halfHeight;
        Vector3 point2 = capsuleCenter - Vector3.up * halfHeight;

        // Perform the CapsuleCastAll
        Collider[] hitColliders = Physics.OverlapCapsule(point1, point2, radius);

        // Check each hit collider to see if it has IsTrigger enabled
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.isTrigger)
            {
                _currentHits.Add(hitCollider);   
                if (!_previousHits.Contains(hitCollider)) {
                    HandleCollision(hitCollider.gameObject);
                }
            }
        }

        _previousHits.Clear();
        var temp = _previousHits;
        _previousHits = _currentHits;
        _currentHits = temp;
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        HandleCollision(hit.gameObject);
    }


    private void HandleCollision(GameObject obj) {
        PlayerActivatable[] playerActivatables = obj.GetComponents<PlayerActivatable>();
        if (playerActivatables != null) {
            foreach (PlayerActivatable playerActivatable in playerActivatables) {
                playerActivatable.OnColliding();
            }
        }
    }
}
