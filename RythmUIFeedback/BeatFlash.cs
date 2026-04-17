using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attach to a full-screen Canvas Image behind everything else.
/// Set the Image color to whatever you want the flash tint to be (e.g. white or warm yellow).
/// Set Image alpha to 0 in the Inspector — this script drives it entirely.
///
/// Setup:
///   1. Create a UI Image, stretch it to fill the whole canvas (anchor: stretch/stretch)
///   2. Set it as the FIRST child of your canvas so it renders behind everything
///   3. Set its color to white (or a subtle warm tint)
///   4. Attach this script
///   5. Match BPM to your song
/// </summary>
public class BeatFlash : MonoBehaviour
{
    [Header("Rhythm")]
    [Tooltip("Match to your song BPM.")]
    public float bpm = 120f;

    [Tooltip("0 = on the beat. Offset if the flash feels off.")]
    [Range(0f, 1f)]
    public float phase = 0f;

    [Header("Flash Intensity")]
    [Tooltip("How bright the flash peaks. Keep this low (0.05–0.18) to stay subtle.")]
    [Range(0f, 0.4f)]
    public float maxAlpha = 0.10f;

    [Tooltip("How sharp the flash is. 4 = soft pulse, 10 = crisp beat flash.")]
    [Range(1f, 16f)]
    public float sharpness = 8f;

    // ── internals ─────────────────────────────────────────────────────────────
    private Image img;
    private Color baseColor;

    // ═════════════════════════════════════════════════════════════════════════
    void Awake()
    {
        img       = GetComponent<Image>();
        baseColor = img != null ? img.color : Color.white;
        baseColor.a = 0f;
    }

    void Update()
    {
        if (img == null) return;

        float beatsPerSec = bpm / 60f;
        float t           = ((Time.time * beatsPerSec) + phase) % 1f;

        // Sharp decay from beat peak → silence (same curve as HitZonePulse)
        float pulse = Mathf.Pow(1f - t, sharpness);

        Color c = baseColor;
        c.a     = Mathf.Lerp(0f, maxAlpha, pulse);
        img.color = c;
    }

    void OnDisable()
    {
        if (img != null)
        {
            Color c = img.color;
            c.a       = 0f;
            img.color = c;
        }
    }
}