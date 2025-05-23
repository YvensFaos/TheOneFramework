using Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class RespawnBehaviour : MonoBehaviour
{
    [Tooltip("Should this object teleport to the spawn point at the start?")]
    public bool teleportOnStart;
    public GameObject spawnPoint = null;
    private Transform respawnPoint;

    private Transform teleportationTarget = null;

    public CinemachineVirtualCamera cinemachineVirtualCamera;

    public float fallThreshold = -10f;

    void Start()
    {
        if (!teleportOnStart)
            return;
        if (spawnPoint != null)
        {
            SetRespawnPoint(spawnPoint);
            Respawn();
        }
        else
        {
            var spawns = GameObject.FindGameObjectsWithTag("SpawnPoint");
            if (spawns.Length > 0)
            {
                var id = Random.Range(0, spawns.Length);
                SetRespawnPoint(spawns[id]);
                Respawn();
            }
        }
    }

    void Update()
    {
        if (transform.position.y < fallThreshold)
        {
            Respawn();
        }
    }

    void LateUpdate()
    {
        if (teleportationTarget != null)
        {
            TeleportImmediatelyTo(teleportationTarget);
            teleportationTarget = null;
        }
    }

    public void SetRespawnPoint(GameObject spawnPoint)
    {
        respawnPoint = spawnPoint.transform;
    }

    public void Respawn()
    {
        TeleportTo(respawnPoint);
    }

    public void Teleport(GameObject target)
    {
        TeleportTo(target.transform);
    }

    public void TeleportTo(Transform targetTransform)
    {
        teleportationTarget = targetTransform;
    }

    private void TeleportImmediatelyTo(Transform targetTransform)
    {
        if (TryGetComponent<NavMeshAgent>(out var navMeshAgent))
        {
            TeleportNPC(targetTransform, navMeshAgent);
        }
        else
        if (TryGetComponent<CharacterController>(out var characterController))
        {
            TeleportPlayer(targetTransform, characterController);
        }
    }

    private void TeleportNPC(Transform targetTransform, NavMeshAgent navMeshAgent)
    {
        var currentCameraPosition = Vector3.zero;
        if (cinemachineVirtualCamera != null)
        {
            currentCameraPosition = cinemachineVirtualCamera.m_Follow.transform.position;
        }

        navMeshAgent.enabled = false;
        navMeshAgent.Warp(targetTransform.position);
        transform.rotation = targetTransform.rotation;
        navMeshAgent.enabled = true;

        if (cinemachineVirtualCamera != null)
        {
            cinemachineVirtualCamera.OnTargetObjectWarped(cinemachineVirtualCamera.m_Follow, cinemachineVirtualCamera.m_Follow.transform.position - currentCameraPosition);
        }
    }

    private void TeleportPlayer(Transform targetTransform, CharacterController characterController)
    {
        var currentCameraPosition = Vector3.zero;
        if (cinemachineVirtualCamera != null)
        {
            currentCameraPosition = cinemachineVirtualCamera.m_Follow.transform.position;
        }

        characterController.enabled = false;
        transform.SetPositionAndRotation(targetTransform.position, targetTransform.rotation);
        characterController.enabled = true;

        if (cinemachineVirtualCamera != null)
        {
            cinemachineVirtualCamera.OnTargetObjectWarped(cinemachineVirtualCamera.m_Follow, cinemachineVirtualCamera.m_Follow.transform.position - currentCameraPosition);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("SpawnPoint"))
        {
            SetRespawnPoint(hit.gameObject);
        }
        if (hit.collider.CompareTag("RespawnPlayers"))
        {
            Respawn();
        }
    }
}
