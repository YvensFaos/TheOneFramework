using UnityEngine;
using StarterAssets;

[RequireComponent(typeof(StarterAssetsInputsAdditionalInputs))]
public class ProximityTriggerActivator : MonoBehaviour
{

    public float castRadius = 3f;
    private StarterAssetsInputsAdditionalInputs _input;
    private ProximityTrigger _currentClosestTrigger;

    private void Awake()
    {
        _input = GetComponent<StarterAssetsInputsAdditionalInputs>();
    }

    private void Update()
    {
        FindClosestReachableTrigger();
        UpdateClosestTrigger();
    }

    private void FindClosestReachableTrigger()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, castRadius, LayerMask.GetMask("ProximityTriggers"));

        var closestDistance = Mathf.Infinity;
        ProximityTrigger closestTrigger = null;

        foreach (var collider in colliders)
        {
            var trigger = collider.GetComponent<ProximityTrigger>();
            if (trigger != null && trigger.IsAvailable())
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTrigger = trigger;
                }
            }
        }
        if (closestTrigger != _currentClosestTrigger) {
            if (_currentClosestTrigger != null) {
                _currentClosestTrigger.SetVisibilityMode(false);
            }
        }
        if (closestTrigger != null) {
            closestTrigger.SetVisibilityMode(true);
        }
        _currentClosestTrigger = closestTrigger;
    }

    private void UpdateClosestTrigger()
    {
        if (_currentClosestTrigger != null)
        {
            _currentClosestTrigger.SetUserInteractionState(_input.interact);
        }
    }
}
