using UnityEngine;

public class IdleBob : MonoBehaviour
{
    public float bobAmount = 0.05f;
    public float frameRate = 6f;

    private Vector3 startPos;
    private float timer;
    private bool goingUp = true;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        timer += Time.deltaTime;

        float frameTime = 1f / frameRate;

        if (timer >= frameTime)
        {
            timer = 0f;

            if (goingUp)
                transform.localPosition =
                    startPos + Vector3.up * bobAmount;
            else
                transform.localPosition =
                    startPos;

            goingUp = !goingUp;
        }
    }
}