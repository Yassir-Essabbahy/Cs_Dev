using UnityEngine;
using UnityEngine.Events;

public class SuitInteracion : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The NPC GameObject")]
    public GameObject NPC;

    [Header("Events")]
    [Tooltip("Fires when the player gets their suit, hook the ObjectiveManager")]
    public UnityEvent onSuitEquipped;

    private bool hasEquippedSuit = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasEquippedSuit)
        {
            EquipSuit();
        }      
    }

    private void EquipSuit()
    {
       hasEquippedSuit = true;

       if (NPC != null)
        {
            NPC.SetActive(false);
        } 

        onSuitEquipped.Invoke();

        Debug.Log("Done!");
    }
}
