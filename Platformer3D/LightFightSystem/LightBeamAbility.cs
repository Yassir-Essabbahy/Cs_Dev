using UnityEngine;
using System.Collections;


public class LightBeamAbility : MonoBehaviour
{
    private LightEnergySystem energy;

    [Header("Costs")]
    public float freezeCost = 25f;
    public float laserCost = 50f;
    public float scareCost = 100f;

    [Header("Visuals")]
    public LineRenderer laserLine;
    public float laserRange = 50f;


    void Start()
    {
        energy = GetComponent<LightEnergySystem>();
    }

    void Update()
    {
        // 1- Freeze (Input: Q)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (energy.CanConsumeLight(freezeCost))
            {
                ShootFreezePoint();
            }
        }

        // 2- Lazer (Right Mouse)
        if (Input.GetMouseButtonDown(1))
        {
            if (energy.CanConsumeLight(laserCost))
            {
                StartCoroutine(FireLaserRoutine());
            }
        }

        // 3- Scare Beam (Input: E)
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (energy.CanConsumeLight(scareCost))
            {
                TriggerScareBlast();
            }
        }
    }


    void ShootFreezePoint()
    {
        Debug.Log("Freeze");
        //TODO : Raycast for a bullet
    }

    IEnumerator FireLaserRoutine()
    {
        Debug.Log("Laser On 2 seconds");
        laserLine.enabled = true;

        float timer = 0f;
        while(timer <2f)
        {
            timer += Time.deltaTime;

            Vector3 startPos = transform.position;
            Vector3 mouseWorldPos = GetMouseWorldPosition();
            Vector3 direction = (mouseWorldPos - startPos).normalized;

            //start from the player's position
            laserLine.SetPosition(0, startPos);
            RaycastHit hit;
            if (Physics.Raycast(startPos, direction, out hit, laserRange))
            {
                laserLine.SetPosition(1, hit.point);
            }
            else
            {
               laserLine.SetPosition(1, startPos + (direction * laserRange));
            }
            yield return null;
        }
        laserLine.enabled = false;
        Debug.Log("Laser Off");
    }

    void TriggerScareBlast()
    {
        Debug.Log("Scare");
        //TODO : Physics.OverlapSphere to find enemies around the player
    }

    Vector3 GetMouseWorldPosition()
    {
        Plane playerPlane = new Plane(Vector3.forward, new Vector3(0,0,transform.position.z));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distance;
        if (playerPlane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }
        return transform.position;
    }
}
