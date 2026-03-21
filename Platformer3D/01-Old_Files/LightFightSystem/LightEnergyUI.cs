using UnityEngine;
using UnityEngine.UI;

public class LightEnergyUI : MonoBehaviour
{
    [Header("References")]
    public Slider lightSlider;

    void OnEnable()
    {
        LightEnergySystem.OnLightChanged += UpdateUI;
    }

    void OnDisable()
    {
        LightEnergySystem.OnLightChanged -= UpdateUI;
    }

    void UpdateUI(float newAmount)
    {
        lightSlider.value = newAmount;
    }
}