using UnityEngine;
using System;

public class LightEnergySystem : MonoBehaviour
{
    [Header("Energy Settings")]
    public float maxLight = 100f;
    public float currentLight;

    [Header("Auto-Recharge Settings")]
    public float rechargeRate = 2f;
    private float rechargeThreshold; // 25% of maxLight

    // Light Messenger to Update UI
    public static Action<float> OnLightChanged;

    void Start()
    {
        rechargeThreshold = maxLight / 4f;

        // Optional: start full
        //currentLight = maxLight/4;

        OnLightChanged?.Invoke(currentLight);
    }

    void Update()
    {
        // If current light is less than 25%, recharge automatically
        if (currentLight < rechargeThreshold)
        {
            AddLight(rechargeRate * Time.deltaTime);
        }

    }

    public bool CanConsumeLight(float amount)
    {
        if (currentLight >= amount)
        {
            currentLight -= amount;
            OnLightChanged?.Invoke(currentLight);
            return true;
        }

        return false;
    }

    public void AddLight(float amount)
    {
        currentLight += amount;
        currentLight = Mathf.Clamp(currentLight, 0f, maxLight);
        OnLightChanged?.Invoke(currentLight);
    }
}