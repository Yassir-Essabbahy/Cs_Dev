using System;
using System.Collections;
using UnityEngine;

public class LightBeamAbility : MonoBehaviour
{
    
    [Header("References")]
    [SerializeField] private LightEnergySystem lightSystem;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LightProjectile lightProjectilePrefab;
    [SerializeField] private LineRenderer beamLine;

    [Header("Input")]
    [SerializeField] private KeyCode lightShotKey = KeyCode.Q;
    [SerializeField] private KeyCode puzzleLaserKey = KeyCode.E;
    [SerializeField] private KeyCode fearBeamKey = KeyCode.R;

    [Header("1) Light Shot")]
    [SerializeField] private float lightShotCost = 15f;
    [SerializeField] private float lightShotSpeed = 20f;
    [SerializeField] private float freezeDuration = 2f;

    [Header("2) Puzzle Laser")]
    [SerializeField] private float puzzleLaserDuration = 2f;
    [SerializeField] private float puzzleLaserRange = 12f;
    [SerializeField] private LayerMask puzzleLaserHitLayers;

    [Header("3) Fear Beam")]
    [SerializeField] private float fearBeamVisualTime = 0.25f;
    [SerializeField] private float fearBeamRange = 10f;
    [SerializeField] private float fearDurationOnEnemies = 3f;
    [SerializeField] private LayerMask fearBeamHitLayers;

    [SerializeField] private Camera aimCamera;

    private bool isUsingBeam;

    private void Reset()
    {
        lightSystem = GetComponent<LightEnergySystem>();
        firePoint = transform;
    }

    private void Awake()
    {
        Debug.Log("LightBeamAbility Awake on " + gameObject.name);

        if (lightSystem == null)
        {
            lightSystem = GetComponent<LightEnergySystem>();
        }
        if (aimCamera == null)
        {
    aimCamera = Camera.main;
            
        
    }

        if (firePoint == null)
        {
            firePoint = transform;
        }

        if (beamLine != null)
        {
            beamLine.positionCount = 2;
            beamLine.enabled = false;
        }

        Debug.Log("lightSystem: " + (lightSystem != null ? lightSystem.name : "NULL"));
        Debug.Log("firePoint: " + (firePoint != null ? firePoint.name : "NULL"));
        Debug.Log("lightProjectilePrefab: " + (lightProjectilePrefab != null ? lightProjectilePrefab.name : "NULL"));
    }

    private void Update()
    {
        if (lightSystem == null)
        {
            Debug.LogWarning("No LightEnergySystem assigned.");
            return;
        }

        if (Input.GetKeyDown(lightShotKey))
        {
            Debug.Log("Q pressed");
            TryUseLightShot();
        }

        if (Input.GetKeyDown(puzzleLaserKey) && !isUsingBeam)
        {
            Debug.Log("E pressed");
            TryUsePuzzleLaser();
        }

        if (Input.GetKeyDown(fearBeamKey) && !isUsingBeam)
        {
            Debug.Log("R pressed");
            TryUseFearBeam();
        }
    }
    private Vector3 GetMouseAimDirection()
{
    if (aimCamera == null)
        return firePoint.forward.normalized;

    Ray ray = aimCamera.ScreenPointToRay(Input.mousePosition);

    // Side-view game: aim on the same Z plane as the fire point
    Plane plane = new Plane(Vector3.forward, new Vector3(0f, 0f, firePoint.position.z));

    if (plane.Raycast(ray, out float enter))
    {
        Vector3 mouseWorld = ray.GetPoint(enter);
        Vector3 dir = mouseWorld - firePoint.position;
        dir.z = 0f;

        if (dir.sqrMagnitude > 0.0001f)
            return dir.normalized;
    }

    return firePoint.forward.normalized;
}

    private void TryUseLightShot()
    {
        Debug.Log("TryUseLightShot called");

        if (lightProjectilePrefab == null)
        {
            Debug.LogWarning("LightProjectile prefab is missing.");
            return;
        }

        Debug.Log("Current light = " + lightSystem.CurrentLight + " / " + lightSystem.MaxLight);

        if (!lightSystem.TrySpendLight(lightShotCost))
        {
            Debug.Log("Not enough light for Light Shot.");
            return;
        }

        Vector3 aimDirection = GetAimDirection();
        Debug.Log("Aim direction = " + aimDirection);

        LightProjectile projectile = Instantiate(
            lightProjectilePrefab,
            firePoint.position,
            Quaternion.LookRotation(aimDirection)
        );

        Debug.Log("Projectile spawned: " + projectile.name);

        projectile.Initialize(aimDirection, lightShotSpeed, freezeDuration, gameObject);
    }

    private void TryUsePuzzleLaser()
    {
        float laserCost = lightSystem.MaxLight * 0.5f;

        if (!lightSystem.TrySpendLight(laserCost))
        {
            Debug.Log("Not enough light for Puzzle Laser.");
            return;
        }

        StartCoroutine(PuzzleLaserRoutine());
    }

    private void TryUseFearBeam()
{
    float fearCost = 40f; // pick your cost

    if (!lightSystem.TrySpendLight(fearCost))
    {
        Debug.Log("Not enough light for Fear Beam.");
        return;
    }

    StartCoroutine(FearBeamRoutine());
}

    private IEnumerator PuzzleLaserRoutine()
    {
        isUsingBeam = true;
        float timer = 0f;

        while (timer < puzzleLaserDuration)
        {
            timer += Time.deltaTime;

            Vector3 origin = firePoint.position;
            Vector3 direction = GetMouseAimDirection();
            Vector3 endPoint = origin + direction * puzzleLaserRange;

            RaycastHit hit;
            bool didHit = Physics.Raycast(origin, direction, out hit, puzzleLaserRange, puzzleLaserHitLayers, QueryTriggerInteraction.Ignore);

            if (didHit)
            {
                endPoint = hit.point;

                LightPuzzleReceiver receiver = hit.collider.GetComponentInParent<LightPuzzleReceiver>();
                if (receiver != null)
                {
                    receiver.ActivateFor(0.15f);
                }
            }

            UpdateBeamVisual(origin, endPoint);
            yield return null;
        }

        StopBeamVisual();
        isUsingBeam = false;
    }

    private IEnumerator FearBeamRoutine()
    {
        isUsingBeam = true;

        Vector3 origin = firePoint.position;
        Vector3 direction = GetAimDirection();

        RaycastHit[] hits = Physics.RaycastAll(origin, direction, fearBeamRange, fearBeamHitLayers, QueryTriggerInteraction.Ignore);
        Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        float beamEndDistance = fearBeamRange;

        foreach (RaycastHit hit in hits)
        {
            EnemyLightReaction enemy = hit.collider.GetComponentInParent<EnemyLightReaction>();

            if (enemy != null)
            {
                enemy.Scare(fearDurationOnEnemies, transform.position);
            }
            else
            {
                beamEndDistance = hit.distance;
                break;
            }
        }

        Vector3 endPoint = origin + direction * beamEndDistance;
        UpdateBeamVisual(origin, endPoint);

        yield return new WaitForSeconds(fearBeamVisualTime);

        StopBeamVisual();
        isUsingBeam = false;
    }

    private Vector3 GetAimDirection()
    {
        return firePoint.forward.normalized;
    }

    private void UpdateBeamVisual(Vector3 startPoint, Vector3 endPoint)
    {
        if (beamLine == null)
        {
            return;
        }

        beamLine.enabled = true;
        beamLine.SetPosition(0, startPoint);
        beamLine.SetPosition(1, endPoint);
    }

    private void StopBeamVisual()
    {
        if (beamLine == null)
        {
            return;
        }

        beamLine.enabled = false;
    }
}