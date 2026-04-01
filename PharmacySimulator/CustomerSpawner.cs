using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [Header("Spawn Setting")]
    public GameObject[] customerPrefabs;
    public Transform spawnPoint;
    public Transform counterTarget;

    [Header("Player Reference")]
    [Tooltip("Drag the object you want the NPC to look at here (usually the Player's Main Camera)")]
    public Transform playerHead;

    private int currentCustomerIndex = 0;
    private GameObject currentActiveCustomer;

    void Start()
    {
        SpawnNextCustomer();
    }

    void Update()
    {
        if (currentActiveCustomer == null && currentCustomerIndex < customerPrefabs.Length)
        {
            SpawnNextCustomer();
        }
    }

    public void SpawnNextCustomer()
    {
        if (customerPrefabs.Length == 0 || spawnPoint == null || counterTarget == null)
        {
            Debug.LogWarning("Missing Reference CustomerSpawner");
            return;
        }

        if (currentCustomerIndex >= customerPrefabs.Length)
        {
            Debug.Log("No More Customers");
            FindObjectOfType<ObjectiveManager>().CompleteObjective();
            return;
        }

        //create the customer
        currentActiveCustomer = Instantiate(customerPrefabs[currentCustomerIndex], spawnPoint.position, spawnPoint.rotation);

        // set look at object
        LookAt npcLookAt = currentActiveCustomer.GetComponent<LookAt>();
        if (npcLookAt != null)
        {
            npcLookAt.LookAtObj = playerHead != null ? playerHead : Camera.main.transform;
        }


        //target destination set
        CustomerMovement movement = currentActiveCustomer.GetComponent<CustomerMovement>();
        if (movement != null)
        {
            movement.targetDestination = counterTarget;
        }
        currentCustomerIndex++;
        
}
}
