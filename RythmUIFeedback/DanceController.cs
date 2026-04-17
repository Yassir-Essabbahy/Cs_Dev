using UnityEngine;
using System.Collections;

public class DanceBrain : MonoBehaviour
{
    public Animator animator;

    public string[] stepNames;

    public string[] poseBeginNames;
    public string[] poseStopNames;

    public float poseChance = 0.25f;

    void Start()
    {
        StartCoroutine(DanceLoop());
    }

    IEnumerator DanceLoop()
    {
        while (true)
        {
            if (Random.value < poseChance)
            {
                yield return StartCoroutine(PlayPose());
            }
            else
            {
                PlayStep();

                yield return new WaitForSeconds(
                    Random.Range(1f, 2f)
                );
            }
        }
    }

    void PlayStep()
    {
        int index =
            Random.Range(0, stepNames.Length);

        animator.Play(stepNames[index]);
    }

    IEnumerator PlayPose()
    {
        int index =
            Random.Range(0, poseBeginNames.Length);

        animator.Play(poseBeginNames[index]);

        // Wait for begin
        yield return new WaitForSeconds(0.5f);

        // Stay looping
        yield return new WaitForSeconds(
            Random.Range(1f, 2f)
        );

        // Stop pose
        animator.Play(poseStopNames[index]);

        yield return new WaitForSeconds(0.5f);
    }
}