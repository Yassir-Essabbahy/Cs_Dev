using UnityEngine;
using TMPro;
using System.Collections;
using Unity.Cinemachine;

public class InteractionText : MonoBehaviour
{
    [Header("UI & Settings")]
    public TextMeshProUGUI InteractText;
    public float InteractionDistance = 5f;
    private bool CanInteract = true;

    [Header("Cameras")]
    public CinemachineCamera PlayerVcam;
    public CinemachineCamera TalkZoomVcam;
    public bool OnConversation;

    [Header("References")]
    public MonoBehaviour PlayerMovement;
    

    void Update()
    {
        if (!CanInteract) return;

        Transform camTransform = Camera.main.transform; 
        Ray ray = new Ray(camTransform.position, camTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, InteractionDistance))
        {
            // --- 1. MEDICINE LOGIC ---
            if (hit.collider.CompareTag("Medicine"))
            {
                // Find the prescription system FIRST
                PrescriptionSystem ps = FindObjectOfType<PrescriptionSystem>();
                
                // ONLY allow interaction IF the customer is actually waiting for medicine
                if (ps != null && ps.isWaitingForMedicine == true)
                {
                    
                    MedicineBottle bottle = hit.collider.GetComponent<MedicineBottle>();

                    if (bottle != null)
                    {
                        if (bottle.medicineName == ps.requiredMedicine)
                        {
                            InteractText.text = "Press 'E' To Pick Up " + bottle.medicineName;
                        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
                        {
                            ps.hasMedicineInHand = true;
                            Debug.Log("Picked up the medicine!");
                            Destroy(hit.collider.gameObject);
                        }
                        }
                        else
                        {
                            InteractText.text = "";
                        }
                    }
                    
                }
                else
                {
                    // If no one is waiting for medicine, pretend it's just a normal unclickable prop
                    InteractText.text = ""; 
                }
            }

            // --- 2. NPC LOGIC ---
            else if (hit.collider.CompareTag("InteractNPC"))
            {
                PrescriptionSystem ps = FindObjectOfType<PrescriptionSystem>();
                if (ps.isWaitingForMedicine) { InteractText.text = "Press 'E' To Give Medicine"; }
                else { InteractText.text = "Press 'E' To Talk"; }
                
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Conversation npcConv = hit.collider.GetComponent<Conversation>();
                    LookAt npcLook = hit.collider.GetComponent<LookAt>();

                    if (npcConv != null)
                    {
                        OnConversation = true;
                        StartCoroutine(TalkSequence(npcConv, npcLook));
                    }
                }
            }

            // --- 3. LOOKING AT A WALL ---
            else 
            { 
                InteractText.text = ""; 
            }
        }
        // --- 4. LOOKING AT THIN AIR ---
        else 
        { 
            InteractText.text = ""; 
        }
    }

    IEnumerator TalkSequence(Conversation conv, LookAt look)
    {
        CanInteract = false;
        InteractText.text = "";

        if (PlayerMovement != null) PlayerMovement.enabled = false;
        if (look != null) look.IKActive = true;

        // Point the talk camera at the NPC's head before activating it
        if (conv.headTarget != null)
        {
            TalkZoomVcam.Target.TrackingTarget = conv.headTarget;
            TalkZoomVcam.LookAt = conv.headTarget;
        }

        PlayerVcam.Priority = 0;
        TalkZoomVcam.Priority = 10;

        yield return new WaitForSeconds(1f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Play dialogue
        yield return StartCoroutine(conv.Play());

        // Check if prescription just popped up
        PrescriptionSystem ps = FindObjectOfType<PrescriptionSystem>();
        if (ps != null &&  ps.prescriptionCanvas != null && ps.prescriptionCanvas.activeSelf)
        {
            yield return new WaitUntil(() => !ps.prescriptionCanvas.activeSelf);
        }

        // Reset
        PlayerVcam.Priority = 10;
        TalkZoomVcam.Priority = 0;
        if (look != null) look.IKActive = false;
        Cursor.lockState = CursorLockMode.Locked;
        OnConversation = false;

        if (PlayerMovement != null) PlayerMovement.enabled = true;
        CanInteract = true;
    }
}