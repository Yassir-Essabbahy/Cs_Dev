using UnityEngine;
using TMPro;
using System.Collections;

public class RhythmComboUI : MonoBehaviour
{
    [Header("Text References")]
    public TMP_Text comboText;          // e.g. "×12 COMBO!"
    public TMP_Text breakText;          // e.g. "COMBO BREAK!"

    [Header("Settings")]
    [Tooltip("Minimum combo count before a miss triggers COMBO BREAK.")]
    public int breakThreshold = 5;

    // ── state ─────────────────────────────────────────────────────────────────
    private int combo = 0;
    private Coroutine comboAnim;
    private Coroutine breakAnim;

    // ── colours ───────────────────────────────────────────────────────────────
    private static readonly Color ColLow   = new Color(1f, 1f,    1f);     
    private static readonly Color ColMid   = new Color(1f, 0.84f, 0f);     
    private static readonly Color ColHigh  = new Color(0.4f, 1f,  0.4f);   
    private static readonly Color ColMax   = new Color(1f, 0.35f, 0.35f);  
    private static readonly Color ColBreak = new Color(1f, 0.2f,  0.2f);

    // ═════════════════════════════════════════════════════════════════════════
    void Awake()
    {
        if (comboText != null) comboText.gameObject.SetActive(false);
        if (breakText != null) breakText.gameObject.SetActive(false);
    }

    // ── Public API ────────────────────────────────────────────────────────────

public void OnHit()
{
    combo++;

    Debug.Log("Combo now: " + combo);

    if (combo >= 3)
        AnimateCombo();
}

    public void OnMiss()
    {
        bool shouldBreak = combo >= breakThreshold;
        combo = 0;

        if (comboAnim != null)
            StopCoroutine(comboAnim);

        if (comboText != null)
            comboText.gameObject.SetActive(false);

        if (shouldBreak)
            TriggerBreak();
    }

    // ── Internal ──────────────────────────────────────────────────────────────

    void AnimateCombo()
    {
        if (comboText == null) return;

        if (comboAnim != null)
            StopCoroutine(comboAnim);

        comboAnim = StartCoroutine(ComboRoutine());
    }

    void TriggerBreak()
    {
        if (breakText == null) return;

        if (breakAnim != null)
            StopCoroutine(breakAnim);

        breakAnim = StartCoroutine(BreakRoutine());
    }

    Color ComboColor()
    {
        if (combo >= 50) return ColMax;
        if (combo >= 25) return ColHigh;
        if (combo >= 10) return ColMid;
        return ColLow;
    }

    // Slight extra boost when combo first appears at 3
    float ComboScale()
    {
        if (combo >= 50) return 1.45f;
        if (combo >= 25) return 1.25f;
        if (combo >= 10) return 1.10f;
        if (combo == 3)  return 1.15f; // First visible combo feels special
        return 1.00f;
    }

    // ═════════════════════════════════════════════════════════════════════════
    //  Coroutines
    // ═════════════════════════════════════════════════════════════════════════

    IEnumerator ComboRoutine()
    {
        RectTransform rt = comboText.rectTransform;
        Color col = ComboColor();
        float baseScale = ComboScale();

        comboText.text  = $"×{combo} COMBO!";
        comboText.color = new Color(col.r, col.g, col.b, 1f);
        comboText.gameObject.SetActive(true);

        // ── Punch in ──────────────────────────────────────────
        float punchScale = baseScale + 0.30f;
        const float punchDur = 0.07f;

        float t = 0f;

        while (t < punchDur)
        {
            t += Time.unscaledDeltaTime;
            float s = Mathf.Lerp(baseScale, punchScale, t / punchDur);
            rt.localScale = new Vector3(s, s, 1f);
            yield return null;
        }

        // ── Settle ────────────────────────────────────────────
        const float settleDur = 0.07f;

        t = 0f;

        while (t < settleDur)
        {
            t += Time.unscaledDeltaTime;
            float s = Mathf.Lerp(punchScale, baseScale, t / settleDur);
            rt.localScale = new Vector3(s, s, 1f);
            yield return null;
        }

        rt.localScale = new Vector3(baseScale, baseScale, 1f);

        // ── Hold + breathe ────────────────────────────────────
        float holdTime = 2.5f;

        t = 0f;

        while (t < holdTime)
        {
            t += Time.unscaledDeltaTime;

            // Subtle breathing effect
            float breathe = 1f + 0.04f * Mathf.Sin(t * 5f);
            rt.localScale = new Vector3(baseScale * breathe, baseScale * breathe, 1f);

            // Fade last 0.5s
            float alpha =
                t > holdTime - 0.5f
                ? Mathf.Lerp(1f, 0f, (t - (holdTime - 0.5f)) / 0.5f)
                : 1f;

            comboText.color = new Color(col.r, col.g, col.b, alpha);

            yield return null;
        }

        comboText.gameObject.SetActive(false);
        rt.localScale = Vector3.one;

        comboAnim = null;
    }

    IEnumerator BreakRoutine()
    {
        RectTransform rt = breakText.rectTransform;
        rt.localScale = Vector3.zero;

        breakText.text  = "COMBO BREAK!";
        breakText.color = new Color(ColBreak.r, ColBreak.g, ColBreak.b, 1f);
        breakText.gameObject.SetActive(true);

        // ── Slam in ───────────────────────────────────────────
        const float inDur = 0.08f;

        float t = 0f;

        while (t < inDur)
        {
            t += Time.unscaledDeltaTime;
            float s = Mathf.SmoothStep(0f, 1.4f, t / inDur);
            rt.localScale = new Vector3(s, s, 1f);
            yield return null;
        }

        // Snap back
        const float snapDur = 0.06f;

        t = 0f;

        while (t < snapDur)
        {
            t += Time.unscaledDeltaTime;
            float s = Mathf.Lerp(1.4f, 1f, t / snapDur);
            rt.localScale = new Vector3(s, s, 1f);
            yield return null;
        }

        rt.localScale = Vector3.one;

        // ── Shake ─────────────────────────────────────────────
        Vector2 origin = rt.anchoredPosition;

        const float shakeDur = 0.35f;

        t = 0f;

        while (t < shakeDur)
        {
            t += Time.unscaledDeltaTime;

            float decay = 1f - (t / shakeDur);
            float x = Mathf.Sin(t * 160f) * 14f * decay;
            float y = Mathf.Sin(t * 120f) * 6f * decay;

            rt.anchoredPosition = origin + new Vector2(x, y);

            yield return null;
        }

        rt.anchoredPosition = origin;

        // ── Fade out ──────────────────────────────────────────
        const float fadeDur = 0.4f;

        t = 0f;

        while (t < fadeDur)
        {
            t += Time.unscaledDeltaTime;

            float alpha = Mathf.Lerp(1f, 0f, t / fadeDur);

            breakText.color =
                new Color(ColBreak.r, ColBreak.g, ColBreak.b, alpha);

            yield return null;
        }

        breakText.gameObject.SetActive(false);

        breakAnim = null;
    }
}