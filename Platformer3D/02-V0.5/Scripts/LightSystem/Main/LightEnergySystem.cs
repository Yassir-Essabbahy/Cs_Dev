using System;
using UnityEngine;

public class LightEnergySystem : MonoBehaviour
{
    public static event Action<float> OnLightChanged;

    [SerializeField] private float maxLight = 100f;
    [SerializeField] private float currentLight = 100f;

    public float MaxLight => maxLight;
    public float CurrentLight => currentLight;

    private void Awake()
    {
        if (maxLight < 0f)
            maxLight = 0f;

        currentLight = Mathf.Clamp(currentLight, 0f, maxLight);

        // If you always want to start full, force it:
        currentLight = maxLight;
    }

    private void Start()
    {
        NotifyLightChanged();
    }

    public bool TrySpendLight(float amount)
    {
        if (currentLight < amount)
            return false;

        currentLight -= amount;
        currentLight = Mathf.Clamp(currentLight, 0f, maxLight);
        NotifyLightChanged();
        return true;
    }

    public void AddLight(float amount)
    {
        currentLight += amount;
        currentLight = Mathf.Clamp(currentLight, 0f, maxLight);
        NotifyLightChanged();
    }

    public void Recharge(float amount)
    {
        AddLight(amount);
    }

    public bool IsFull()
    {
        return currentLight >= maxLight;
    }

    private void NotifyLightChanged()
    {
        OnLightChanged?.Invoke(currentLight);
    }
}