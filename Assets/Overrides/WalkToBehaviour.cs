using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;

[RequireComponent(typeof(NavMeshAgent))]
//[RequireComponent(typeof(Animator))]
public class WalkToBehaviour : MonoBehaviour {
    
    private Animator animator;
    private NavMeshAgent agent;

    private float speed = 0f;

    public GameObject target = null;
    public float targetDistance = 0f;

    void Awake() {
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        animator = GetComponent<Animator>();
    }    

    void UpdateTargetPosition() {
        if (target != null) {
            agent.destination = target.transform.position - target.transform.forward * targetDistance;
        }
    }

    void UpdatePathfinding() {
        UpdateTargetPosition();

        if (!agent.pathPending)
        if (agent.remainingDistance <= agent.stoppingDistance) {
            target = null;
            targetDistance = 0f;
        }

        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

        float newSpeed = worldDeltaPosition.magnitude * 1000f;
        speed = speed * 0.9f + newSpeed * 0.1f;
        if (speed >= 5f) { speed = 5f; }

        if (animator != null) {
            animator.SetFloat("MotionSpeed", speed * 0.2f);
            animator.SetFloat("Speed", speed);
        }

        transform.position = agent.nextPosition;
        if (agent.velocity != Vector3.zero) {
            transform.forward = Vector3.Slerp(transform.forward, agent.velocity.normalized, 0.015f);
        }
    }

    public IEnumerator WalkTo(GameObject destination=null, float distance=0f, bool shouldWait = true) {
        target = destination;
        targetDistance = distance;
        if (shouldWait && destination != null) {
            UpdateTargetPosition();
            while (target != null) {
                yield return null;
            }
        }
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        //footstep
    }

    void Update() {
        UpdateTargetPosition();
        UpdatePathfinding();
    }

}