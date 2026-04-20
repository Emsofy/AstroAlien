using UnityEngine;
using UnityEngine.AI;

public class BaseChicks : MonoBehaviour
{
    public enum AIState
    {
        Patrol,
        
    }
    private NavMeshAgent agent;
    private AIState currentState;

    [Header("Traits")]
    public float patrolSpeed = 2f;

    [Header("Wander Settings")]
    public float wanderRadius = 3f;
    public float wanderMinDistance = 2f;
    public float idleTime = 2f;

    private float idleTimer;
    private Vector3 lastDestination;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ChangeState(AIState.Patrol);
        //anim = GetComponent<Animator>();
        //Debug.Log("On NavMesh: " + agent.isOnNavMesh);
        idleTimer = UnityEngine.Random.Range(1f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case AIState.Patrol:
                //Debug.Log("In patrol");
                UpdatePatrol();
                break;

        }
    }
    void UpdatePatrol()
    {
        // If we've reached destination
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            idleTimer -= Time.deltaTime;

            if (idleTimer <= 0f)
            {
                Wander();
                idleTimer = UnityEngine.Random.Range(1f, 3f);
            }
        }
    }
    void Wander()
    {
        //attempts for finding spot (10)
        for (int i = 0; i < 10; i++)
        {
            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * wanderRadius;
            Vector3 randomPoint = new Vector3(randomCircle.x, 0, randomCircle.y);
            Vector3 candidate = transform.position + randomPoint;

            NavMeshHit hit;

            if (NavMesh.SamplePosition(candidate, out hit, wanderRadius, NavMesh.AllAreas))
            {
                if (NavMesh.FindClosestEdge(hit.position, out NavMeshHit edgeHit, NavMesh.AllAreas))
                {
                    if (edgeHit.distance < 2.0f)
                        continue;
                }

                float distance = Vector3.Distance(transform.position, hit.position);
                if (distance < wanderMinDistance)
                    continue;

                agent.SetDestination(hit.position);
                lastDestination = hit.position;
                return;
            }
        }
        //fallback
        Vector3 fallback = transform.position + transform.forward * 2f;
        agent.SetDestination(fallback);
    }
    void ChangeState(AIState newState)
    {
        currentState = newState;
        switch(newState)
        {
            case AIState.Patrol:
                agent.speed = patrolSpeed;
                idleTimer = 0f;
                Wander();
                break;
        }
    }
}
