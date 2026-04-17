using System;
using UnityEngine;
using UnityEngine.AI;

public class ChickenMov : MonoBehaviour
{
    public enum AIState
    {
        Patrol,
        LayEgg,
        Follow
    }
    //public Transform player;
    //public GameObject playerBody;

    [Header("Patrol")]
    //public float waypointTolerance = 1f;
    //public float patrolRadius = 5f;

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
    //public Animator anim;

    [Header("Bools")]
    //public bool isIdle = false;
    public bool isWalking = false;
    public bool eggLayed = false;

    [Header("Egg Laying")]
    public GameObject eggPrefab;
    public GameObject goldEggPrefab;
    //private DateTime layTime;
    //public float layIntervalSeconds = 120f; // 2 minutes

    //[Header("Follow Settings")]
    //public float followDistance = 2f;
    //public float followSpeed = 3.5f;

    //private bool isFollowing = false;

    private DateTime nextEggTime;
    private TimeSpan layDuration;
    public string id;

    public void Init (ChickenSaveData data)
    {
        id = data.id;
        transform.position = data.position;
        nextEggTime = new DateTime(data.nextEggTicks);
        layDuration = TimeSpan.FromMinutes(3);
        Debug.Log("Running chicken init");
        // Initialize NavMeshAgent
        agent = GetComponent<NavMeshAgent>();

        // Ensure the agent is enabled
        if (agent != null)
        {
            agent.enabled = true;
        }

        // Safely place the chicken on the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(data.position, out hit, 2f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }
        Debug.Log("Running chicken init");


    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartNew(Vector3 position)
    {
        id = Guid.NewGuid().ToString();
        transform.position = position;

        agent = GetComponent<NavMeshAgent>();
        ChangeState(AIState.Patrol);
        //anim = GetComponent<Animator>();
        Debug.Log("On NavMesh: " + agent.isOnNavMesh);
        idleTimer = UnityEngine.Random.Range(1f, 3f);
        layDuration = TimeSpan.FromMinutes(1);
        nextEggTime = DateTime.UtcNow.Add(layDuration);
        Debug.Log("Created new chicken stuff");

    }

    // Update is called once per frame
    void Update()
    {
        TimeSpan remaining = nextEggTime - DateTime.UtcNow;
        //Clamping so no neg times
        if (remaining < TimeSpan.Zero)
            remaining = TimeSpan.Zero;
        if (remaining <= TimeSpan.Zero)
        {
            LayEgg();
            eggLayed = true;
        }
        switch (currentState)
        {
            case AIState.Patrol:
                //Debug.Log("In patrol");
                UpdatePatrol();
                break;


            //case AIState.Follow:
            //    UpdateFollow();
                
            //    break;
        }
        //UpdatePatrol();
        //UpdateLayEgg();

        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    float dist = Vector3.Distance(transform.position, player.position);

        //    if (dist < 5f) // only allow if near player
        //    {
        //        isFollowing = !isFollowing;

        //        if (isFollowing)
        //            ChangeState(AIState.Follow);
        //        else
        //            ChangeState(AIState.Patrol);
        //    }
        //}
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

    void LayEgg()
    {
        if (!eggLayed)
        {
            int eggProb = UnityEngine.Random.Range(1, 11);
            Debug.Log(eggProb);
            if (eggProb > 2)
            {
                Vector3 spawnPos = transform.position + transform.right * UnityEngine.Random.Range(-0.5f, 0.5f);
                Instantiate(eggPrefab, spawnPos, Quaternion.identity);
                Debug.Log("Egg laid");

            }
            else
            {
                Vector3 spawnPos = transform.position + transform.right * UnityEngine.Random.Range(-0.5f, 0.5f);
                Instantiate(goldEggPrefab, spawnPos, Quaternion.identity);
                Debug.Log("Golden egg laid");
            }
            SaveSystem.SaveGame();
        }
        else
        {
            //Debug.Log("Egg already layed");
            return;
        }
    }    
    //void UpdateFollow()
    //{
    //    //if player comes up to chicken and presses f the chicken begins to follow until f is pressed again
    //    float dist = Vector3.Distance(transform.position, player.position);
    //    //agent.stoppingDistance = followDistance;
    //    if (dist > followDistance)
    //    {
    //        agent.speed = followSpeed;
    //        agent.SetDestination(player.position);
    //    }
    //    else
    //    {
    //        agent.ResetPath(); // stop when close
    //    }
    //}
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
    public ChickenSaveData GetSaveData()
    {
        Debug.Log("creating new chicken save");
        return new ChickenSaveData
        {
            id = id,
            position = transform.position,
            nextEggTicks = nextEggTime.Ticks

        };
       
    }
    public void SetRemainingSeconds (int seconds)
    {
        nextEggTime = DateTime.UtcNow.AddSeconds(seconds);
    }
    public int GetRemainingSeconds()
    {
        return (int)(nextEggTime -  DateTime.UtcNow).TotalSeconds;
    }
}

//Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
//randomDirection += transform.position;

//NavMeshHit hit;
//Vector3 finalPosition = Vector3.zero;

//// Find the nearest valid point on the NavMesh within the specified radius
//if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1))
//{
//    finalPosition = hit.position;
//}

//return;

//anim.SetBool("isWalking", true);
//for (int i = 0; i < 5; i++)
//{
//    //Bias movement forward instead of fully random
//   Vector3 randomDirection = transform.forward * UnityEngine.Random.Range(2f, wanderRadius);
//randomDirection += UnityEngine.Random.insideUnitSphere * wanderRadius * 0.5f;
//randomDirection.y = 0;

//Vector3 candidate = transform.position + randomDirection;

//NavMeshHit hit;

//if (NavMesh.SamplePosition(candidate, out hit, wanderRadius, NavMesh.AllAreas))
//{
//    float distance = Vector3.Distance(transform.position, hit.position);

//    //// Avoid very close points
//    //if (distance < wanderMinDistance)
//    //    continue;

//    //// Avoid going back to last location
//    //if (Vector3.Distance(hit.position, lastDestination) < wanderMinDistance)
//    //    continue;

//    agent.SetDestination(hit.position);
//    if (agent.pathStatus == NavMeshPathStatus.PathPartial)
//        return;

//    lastDestination = hit.position;
//    return;
//}
//}
//things to save
// - if egg is layed and which kind of egg
// how many chickens in pen and can save their last pos but dont have to
//set up pen so chickens only lay eggs in there 
