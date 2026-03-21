using System.Collections;
using UnityEngine;

public class LightPuzzleReceiver : MonoBehaviour
{
    [Header("Objects To Control")]
    [SerializeField] private GameObject objectToEnable;
    [SerializeField] private GameObject objectToDisable;

    private Coroutine activeRoutine;

    public void ActivateFor(float duration)
    {
        if (activeRoutine != null)
        {
            StopCoroutine(activeRoutine);
        }

        activeRoutine = StartCoroutine(ActivationRoutine(duration));
    }

    private IEnumerator ActivationRoutine(float duration)
    {
        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
        }

        if (objectToDisable != null)
        {
            objectToDisable.SetActive(false);
        }

        yield return new WaitForSeconds(duration);

        if (objectToEnable != null)
        {
            objectToEnable.SetActive(false);
        }

        if (objectToDisable != null)
        {
            objectToDisable.SetActive(true);
        }

        activeRoutine = null;
    }
}
