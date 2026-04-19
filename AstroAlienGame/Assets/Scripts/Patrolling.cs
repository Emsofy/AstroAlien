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
        if (DialogueManager.AnyDialogueRunning)
        {
            agent.isStopped = true;

            // --- NEW ROTATION LOGIC ---
            // Find the player (ensure your Player object is tagged "Player")
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                // Calculate the direction to the player
                Vector3 direction = (player.transform.position - transform.position).normalized;

                // Set Y to 0 so the alien doesn't tilt up or down if the player is taller/shorter
                direction.y = 0;

                // Create the rotation we want
                Quaternion lookRotation = Quaternion.LookRotation(direction);

                // Smoothly rotate towards that direction over time
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
            // ---------------------------

            return;
        }

        agent.isStopped = false;

        if (waypoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[waypointIndex].position);
        }
    }
}