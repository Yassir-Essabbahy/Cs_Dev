using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CustomerMovement : MonoBehaviour
{

    [Header("References")]
    public Animator animator;

    [Header("Navigation")]
    public Transform targetDestination;
    private NavMeshAgent agent;
    private bool hasArrived = false;
    private bool isLeaving = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (animator == null) animator = GetComponentInChildren<Animator>();

        // FORCE the agent onto the NavMesh instantly to prevent errors
        agent.Warp(transform.position);

        // Safety check before setting destination
        if (targetDestination != null && agent.isOnNavMesh)
        {
            agent.SetDestination(targetDestination.position);
        }
    }

    void Update()
    {
        // SAFETY CHECK: If the agent isn't fully on the mesh yet, do nothing. 
        // This completely stops the "GetRemainingDistance" error!
        if (!agent.isOnNavMesh) return;

        // ----Animator LOGIC-------------
        if (animator != null)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }

        //check if we still moving
        if (!hasArrived && targetDestination != null)
        {
            // Now it is safe to check the remaining distance
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                if (isLeaving)
                {
                    Destroy(gameObject);   
                }
                else
                {
                ArrivedAtCounter();
                    
                }
            }
        }         
    }

    private void ArrivedAtCounter()
    {
        hasArrived = true;

        //Stop the agent
        agent.isStopped = true;
        agent.updateRotation = false;

        //rotate to face the player
        if (targetDestination != null)
        {
            transform.rotation = targetDestination.rotation; 
        }
        //enable interaction
        gameObject.tag = "InteractNPC";

        Debug.Log("Customer has arrived at the counter.");

        // TODO : BELL SOUND HERE 
    }

    public void LeaveStore(Transform exitPoint)
    {
        isLeaving = true;
        hasArrived = false;

        agent.isStopped = false;
        agent.updateRotation = true;
        
        gameObject.tag = "Untagged";

        targetDestination = exitPoint;
        agent.SetDestination(exitPoint.position);
    }
}