using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLightReaction : MonoBehaviour
{
    [Header("Optional References")]
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private MonoBehaviour movementScriptToDisable;

    [Header("Scare")]
    [SerializeField] private float scaredMoveSpeed = 4f;
    [SerializeField] private float scaredRunDistance = 6f;

    private Coroutine freezeRoutine;
    private Coroutine scareRoutine;
    private RigidbodyConstraints savedConstraints;
    private bool savedAgentStopped;

    private void Awake()
{
    if (navMeshAgent == null)
        navMeshAgent = GetComponent<NavMeshAgent>();

    if (rb == null)
        rb = GetComponent<Rigidbody>();

    if (movementScriptToDisable == null)
        movementScriptToDisable = GetComponent<EnemyAI>();

    if (rb != null)
        savedConstraints = rb.constraints;
}

    /*public void Freeze(float duration)
    {
        if (freezeRoutine != null)
        {
            StopCoroutine(freezeRoutine);
        }

        if (scareRoutine != null)
        {
            StopCoroutine(scareRoutine);
            scareRoutine = null;
        }

        freezeRoutine = StartCoroutine(FreezeRoutine(duration));
    }
*/
    public void Scare(float duration, Vector3 dangerSource)
{
    if (freezeRoutine != null)
    {
        StopCoroutine(freezeRoutine);
        freezeRoutine = null;

        if (rb != null)
            rb.constraints = savedConstraints;

        if (navMeshAgent != null)
            navMeshAgent.isStopped = savedAgentStopped;
    }

    if (scareRoutine != null)
        StopCoroutine(scareRoutine);

    scareRoutine = StartCoroutine(ScareRoutine(duration, dangerSource));
}

    public void Freeze(float duration)
{
    if (scareRoutine != null)
    {
        StopCoroutine(scareRoutine);
        scareRoutine = null;
    }

    // First freeze hit: apply freeze state
    if (freezeRoutine == null)
    {
        SetMovementEnabled(false);

        if (navMeshAgent != null)
        {
            savedAgentStopped = navMeshAgent.isStopped;
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            savedConstraints = rb.constraints;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
    else
    {
        // Already frozen: just restart the timer
        StopCoroutine(freezeRoutine);
    }

    freezeRoutine = StartCoroutine(FreezeCountdown(duration));
}

private IEnumerator FreezeCountdown(float duration)
{
    yield return new WaitForSeconds(duration);

    if (rb != null)
        rb.constraints = savedConstraints;

    if (navMeshAgent != null)
        navMeshAgent.isStopped = savedAgentStopped;

    SetMovementEnabled(true);
    freezeRoutine = null;
}

    private IEnumerator ScareRoutine(float duration, Vector3 dangerSource)
    {
        SetMovementEnabled(false);

        Vector3 awayDirection = transform.position - dangerSource;
        awayDirection.y = 0f;

        if (awayDirection.sqrMagnitude < 0.01f)
        {
            awayDirection = transform.forward;
        }

        awayDirection.Normalize();
        float timer = duration;

        float originalAgentSpeed = 0f;
        if (navMeshAgent != null)
        {
            originalAgentSpeed = navMeshAgent.speed;
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = scaredMoveSpeed;
        }

        while (timer > 0f)
        {
            timer -= Time.deltaTime;

            if (navMeshAgent != null)
            {
                navMeshAgent.SetDestination(transform.position + awayDirection * scaredRunDistance);
            }
            else if (rb != null)
            {
                rb.linearVelocity = new Vector3(awayDirection.x * scaredMoveSpeed, rb.linearVelocity.y, awayDirection.z * scaredMoveSpeed);
            }
            else
            {
                transform.position += awayDirection * scaredMoveSpeed * Time.deltaTime;
            }

            yield return null;
        }

        if (navMeshAgent != null)
        {
            navMeshAgent.speed = originalAgentSpeed;
            navMeshAgent.ResetPath();
        }

        SetMovementEnabled(true);
        scareRoutine = null;
    }

    private void SetMovementEnabled(bool enabledValue)
    {
        if (movementScriptToDisable != null)
        {
            movementScriptToDisable.enabled = enabledValue;
        }
    }
}
