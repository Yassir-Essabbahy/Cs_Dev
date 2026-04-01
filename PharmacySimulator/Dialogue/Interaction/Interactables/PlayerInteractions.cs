using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{


void Update()
{
    if (Input.GetKeyDown(KeyCode.E))
    {
        CheckForInteraction();
    }
}

void CheckForInteraction()
{
    RaycastHit hit;
    // Shoot a ray 5 meters forward from the camera/player
    if (Physics.Raycast(transform.position, transform.forward, out hit, 5f))
    {
        // Look for the door script on whatever we hit
        Door door = hit.collider.GetComponent<Door>();

        if (door != null)
        {
            door.ToggleDoor();
        }
    }
}
}
