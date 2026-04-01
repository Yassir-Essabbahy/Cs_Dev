using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    public float openAngle = 90f;
    public float smoothSpeed = 5f;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = transform.localRotation;
        openRotation = Quaternion.Euler(0, openAngle, 0) * closedRotation;
    }

    void Update()
    {
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smoothSpeed);
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
    }
}
