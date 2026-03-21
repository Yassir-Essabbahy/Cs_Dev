using UnityEngine;

public class LightRechargeSource : MonoBehaviour
{
    [SerializeField] private float rechargeAmount = 25f;
    [SerializeField] private bool destroyAfterUse = true;

    private void OnTriggerEnter(Collider other)
    {
        LightEnergySystem lightSystem = other.GetComponentInParent<LightEnergySystem>();
        if (lightSystem == null)
        {
            return;
        }

        lightSystem.Recharge(rechargeAmount);

        if (destroyAfterUse)
        {
            Destroy(gameObject);
        }
    }
}
