using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Yarn.Unity;

public class RespawnBehaviour : MonoBehaviour
{
    public GameObject spawnPoint = null;
    private Vector3 respawnPoint;

    public CinemachineVirtualCamera cinemachineVirtualCamera;

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

    public void SetRespawnPoint(GameObject spawnPoint)
    {
        respawnPoint = spawnPoint.transform.position;
    }

    public void Respawn()
    {
        TeleportTo(respawnPoint);
    }

    public void Teleport(GameObject target) {
        TeleportTo(target.transform.position);
    }

    public void TeleportTo(Vector3 targetPosition)
    {
        var characterController = GetComponent<CharacterController>();
        if (characterController != null) {
            var currentCameraPosition = Vector3.zero;
            if (cinemachineVirtualCamera != null) {
                currentCameraPosition = cinemachineVirtualCamera.m_Follow.transform.position;
            }

            characterController.enabled = false;
            transform.position = targetPosition;
            characterController.enabled = true;

            if (cinemachineVirtualCamera != null) {
                cinemachineVirtualCamera.OnTargetObjectWarped(cinemachineVirtualCamera.m_Follow, cinemachineVirtualCamera.m_Follow.transform.position - currentCameraPosition);
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
