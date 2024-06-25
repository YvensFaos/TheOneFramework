using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlayerActivatables : MonoBehaviour
{   
    private List<PlayerActivatable> _previousHits = new List<PlayerActivatable>();
    private List<PlayerActivatable> _currentHits = new List<PlayerActivatable>();

    void Update() {
        foreach (PlayerActivatable playerActivatable in _currentHits) {
            if (!_previousHits.Contains(playerActivatable)) {
                playerActivatable.OnColliding();
            }
        }
        _previousHits = _currentHits;
        _currentHits = new List<PlayerActivatable>();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        PlayerActivatable[] playerActivatables = hit.gameObject.GetComponents<PlayerActivatable>();
        foreach (PlayerActivatable playerActivatable in playerActivatables) {
            _currentHits.Add(playerActivatable);
        }
    }
}
