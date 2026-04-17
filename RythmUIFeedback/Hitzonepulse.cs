using UnityEngine;

/// <summary>
/// Attach to any Canvas Image (the hit-zone arrows at the bottom of each lane).
/// Produces a sharp beat-punch scale that snaps up on the beat and falls off quickly.
///
/// TIK-TAK setup:
///   Lane 1 arrow  →  phase = 0.0
///   Lane 2 arrow  →  phase = 0.5
///   Lane 3 arrow  →  phase = 0.0
///   Lane 4 arrow  →  phase = 0.5
/// All four can share the same BPM value.
/// </summary>
public class HitZonePulse : MonoBehaviour
{
    [Header("Rhythm")]
    [Tooltip("Match this to your song BPM.")]
    public float bpm = 120f;

    [Tooltip("0 = on the beat  |  0.5 = off the beat (use for tik-TAK alternation)")]
    [Range(0f, 1f)]
    public float phase = 0f;

    [Header("Scale")]
    [Tooltip("Resting scale (between beats).")]
    public float minScale = 0.88f;

    [Tooltip("Peak scale right on the beat.")]
    public float maxScale = 1.22f;

    [Header("Feel")]
    [Tooltip("How sharp the punch is. 2 = smooth sine, 6 = crisp tick, 12 = very snappy.")]
    [Range(1f, 16f)]
    public float sharpness = 6f;

    // ── internals ─────────────────────────────────────────────────────────────
    private RectTransform rt;
    private Vector3 originalScale;

    // ═════════════════════════════════════════════════════════════════════════
    void Awake()
    {
        rt            = GetComponent<RectTransform>();
        originalScale = rt.localScale;
    }

    void Update()
    {
        float beatsPerSec = bpm / 60f;

        // t goes 0→1 over one beat, shifted by phase
        float t = ((Time.time * beatsPerSec) + phase) % 1f;

        // Sharp exponential decay from the beat peak: 1 at t=0, falls quickly
        // Raising (1-t) to a high power gives that snappy tik feel
        float pulse = Mathf.Pow(1f - t, sharpness);

        float s = Mathf.Lerp(minScale, maxScale, pulse);
        rt.localScale = new Vector3(s, s, 1f);
    }

    void OnDisable()
    {
        // Reset cleanly when the object is toggled off
        if (rt != null)
            rt.localScale = originalScale;
    }
}