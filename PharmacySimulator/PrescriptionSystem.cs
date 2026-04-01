using UnityEngine;
using TMPro;
using System.Collections;


public class PrescriptionSystem : MonoBehaviour
{
    [Header("UI References")]
    public GameObject prescriptionCanvas;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI drugText;
    public TextMeshProUGUI dateText;
    public TextMeshProUGUI reasonText;

    [Header("Player Settings")]
    public MonoBehaviour playerMovementScript;

    [Header("Store Settings")]
    [Tooltip("Where should the customer walk away")]
    public Transform exitPoint;

    [Header("Game State")]
    public bool isWaitingForMedicine = false;
    public bool hasMedicineInHand = false;

    [Header("Deny Effects")]
    public Light mainRoomLight;
    public AudioSource environmentAudio;
    public AudioClip coughSound;

    public string requiredMedicine;
    private bool hasGivenPrescription = false;

    public void ShowPrescription(string pName, string pDrug, string pDate, string pReason)
    {

        if (isWaitingForMedicine)
        {
            if (hasMedicineInHand)
            {
                Debug.Log("SUCCESS: you handed the medicine");
                hasMedicineInHand = false;
                isWaitingForMedicine = false;
                DismissCustomer();
            }
            else
            {
                Debug.Log("Customer still waiting for medecine");
            }
            return;
        }


        if (hasGivenPrescription == false)
        {
            requiredMedicine = pDrug;
            
        nameText.text = "Patient: " + pName;
        drugText.text = "Prescription: " + pDrug;
        dateText.text = "Date: " + pDate;
        reasonText.text = "The Patient is suffering from " + pReason;

        //Freeze Player
        if(playerMovementScript != null) playerMovementScript.enabled = false;

        //Show the UI
        prescriptionCanvas.SetActive(true);

        //Unlock Mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        hasGivenPrescription = true;
        }
    }

    //Called when the accept button is clicked
    public void OnAcceptClicked()
    {
      Debug.Log("Accepted, go get medecine");
        ClosePrescriptionUI();

      // Customer Logic HERE
        isWaitingForMedicine = true;

      

    }

    public void OnDenyClicked()
    {
        Debug.Log("You denied the prescription");
        ClosePrescriptionUI();

        // Customer HORROR LOGIC HERE
        //isWaitingForMedicine = false;
        //DismissCustomer();

        DenyCustomer();


    }

    private void DismissCustomer()
    {
        GameObject customer = GameObject.FindGameObjectWithTag("InteractNPC");

        if (customer != null)
        {
            CustomerMovement movement = customer.GetComponent<CustomerMovement>();
            if (movement != null && exitPoint != null)
            {
                movement.LeaveStore(exitPoint);
            }
        }
    }

    private void ClosePrescriptionUI()
    {
        //Hide UI
        prescriptionCanvas.SetActive(false);

        //lock the mouse back
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Unfreeze the player
        if (playerMovementScript != null) playerMovementScript.enabled = true;

        hasGivenPrescription = false;
    }

    public void DenyCustomer()
    {
        StartCoroutine(DenySequence());
    }
private IEnumerator DenySequence()
    {
        // 1. FREEZE AND SPEAK
        InteractionText ui = FindObjectOfType<InteractionText>();
        if (ui != null)
        {
            ui.enabled = false; // NEW: Turn off the raycast script so it stops updating!
            ui.InteractText.color = Color.red;
            ui.InteractText.text = "\"Are you sure?\"";
        }

        // wait for 2 seconds
        yield return new WaitForSeconds(2f);

        // clear text and wake it back up
        if (ui != null) 
        {
            ui.InteractText.text = "";
            ui.InteractText.color = Color.white; // NEW: Make sure it goes back to white!
            ui.enabled = true; // NEW: Turn the raycasts back on!
        }

        // Leave Slowly
        CustomerMovement currentCustomer = FindObjectOfType<CustomerMovement>();
        if (currentCustomer != null)
        {
            //Half of the speed on nav mesh
            UnityEngine.AI.NavMeshAgent agent = currentCustomer.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null) agent.speed = agent.speed/2f;
        }

        //Reset the ui
        isWaitingForMedicine = false;
        requiredMedicine = "";
        hasGivenPrescription = false;
        DismissCustomer();

        // Light Flickers
        if (mainRoomLight != null)
        {
            float originalIntensity = mainRoomLight.intensity;

            //Flicker 10 times
            for (int i = 0; i < 10 ; i++)
            {
                mainRoomLight.intensity = Random.Range(0.1f,0.5f);
                yield return new WaitForSeconds(Random.Range(0.05f,0.15f));
            }

            // back to normal
            mainRoomLight.intensity = originalIntensity;
        }

        //wait for them to go outside
        yield return new WaitForSeconds(1f);

        //cough from outside
        if (environmentAudio != null && coughSound != null)
        {
            environmentAudio.PlayOneShot(coughSound);
        }
    }





}
