using UnityEngine;
using UnityEngine.UI;

public class LightEnergyUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider lightSlider;
    [SerializeField] private LightEnergySystem lightSystem;

    private void Start()
    {
        if (lightSystem == null)
        {
            lightSystem = FindFirstObjectByType<LightEnergySystem>();
        }

        if (lightSystem != null && lightSlider != null)
        {
            lightSlider.minValue = 0f;
            lightSlider.maxValue = lightSystem.MaxLight;
            lightSlider.value = lightSystem.CurrentLight;
        }
    }

    private void OnEnable()
    {
        LightEnergySystem.OnLightChanged += UpdateUI;
    }

    private void OnDisable()
    {
        LightEnergySystem.OnLightChanged -= UpdateUI;
    }

    private void UpdateUI(float newAmount)
    {
        if (lightSlider != null)
        {
            lightSlider.value = newAmount;
        }
    }
}