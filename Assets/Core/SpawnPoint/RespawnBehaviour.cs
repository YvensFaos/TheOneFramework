using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using Yarn.Unity;

public class RespawnBehaviour : MonoBehaviour
{
    public GameObject spawnPoint = null;
    private Transform respawnPoint;

    private Transform teleportationTarget = null;

    public CinemachineCamera cinemachineCamera;

    public float fallThreshold = -10f;

    void Start()
    {
        if (spawnPoint != null) {
            SetRespawnPoint(spawnPoint);
            Respawn();
        } else {
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
        if (teleportationTarget != null) {
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

    public void Teleport(GameObject target) {
        TeleportTo(target.transform);
    }

    public void TeleportTo(Transform targetTransform)
    {
        teleportationTarget = targetTransform;
    }

    private void TeleportImmediatelyTo(Transform targetTransform)
    {
        var characterController = GetComponent<CharacterController>();
        if (characterController != null) {
            var currentCameraPosition = Vector3.zero;
            if (cinemachineCamera != null) {
                currentCameraPosition = cinemachineCamera.Follow.transform.position;
            }

            characterController.enabled = false;
            transform.position = targetTransform.position;
            transform.rotation = targetTransform.rotation;
            characterController.enabled = true;

            if (cinemachineCamera != null) {
                cinemachineCamera.OnTargetObjectWarped(cinemachineCamera.Follow, cinemachineCamera.Follow.transform.position - currentCameraPosition);
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("SpawnPoint")) {
            SetRespawnPoint(hit.gameObject);
        }
        if (hit.collider.CompareTag("RespawnPlayers")) {
            Respawn();
        }
    }
}
