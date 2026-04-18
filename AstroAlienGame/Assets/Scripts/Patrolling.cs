using UnityEngine;
using UnityEngine.AI;

public class Patrolling : MonoBehaviour
{
    public Transform[] waypoints; // Array of waypoints
    private int waypointIndex;
    private NavMeshAgent agent;

    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        if (waypoints.Length > 0) agent.SetDestination(waypoints[0].position);
    }

    void Update()
    {
        if (waypoints.Length == 0) return;

        // Check if agent has reached destination
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Length; // Cycle through points
            agent.SetDestination(waypoints[waypointIndex].position);
        }
    }
}