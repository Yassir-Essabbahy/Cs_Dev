using UnityEngine;
using System.Collections;

public class LightBeamAbility : MonoBehaviour
{
    private LightEnergySystem energy;

    [Header("Costs")]
    public float freezeCost = 15f;
    public float laserCost = 50f;
    public float scareCost = 100f;

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
        //TODO : Enable a lineRenderer

        yield return new WaitForSeconds(2f);

        Debug.Log("Laser Off");
    }

    void TriggerScareBlast()
    {
        Debug.Log("Scare");
        //TODO : Physics.OverlapSphere to find enemies around the player
    }
}
