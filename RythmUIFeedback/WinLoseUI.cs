using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Victory and Game Over overlays.
/// Uses Time.unscaledDeltaTime everywhere so it runs while Time.timeScale = 0.
///
/// ── UNITY HIERARCHY TO BUILD ─────────────────────────────────────────────
///
///  [New Canvas]  Sort Order 10  (sits above your game canvas)
///  └── WinLoseUI            ← attach this script here
///      │
///      ├── VictoryPanel     GameObject, add CanvasGroup, start INACTIVE
///      │   ├── Background       Image  black  alpha 0.88  full-stretch
///      │   ├── WhiteFlash       Image  white  alpha 0     full-stretch  ← victoryFlash
///      │   ├── TitleText        TMP_Text  "VICTORY!"  start INACTIVE    ← victoryTitle
///      │   ├── ScorePanel       GameObject, add CanvasGroup, start INACTIVE  ← scorePanel
///      │   │   ├── PerfectText  TMP_Text    ← perfectCountText
///      │   │   ├── GoodText     TMP_Text    ← goodCountText
///      │   │   └── MissText     TMP_Text    ← missCountText
///      │   ├── ButtonPanel      GameObject, add CanvasGroup, start INACTIVE  ← buttonPanel
///      │   │   ├── PlayAgainBtn Button → WinLoseUI.OnPlayAgain()
///      │   │   └── MainMenuBtn  Button → WinLoseUI.OnMainMenu()
///      │   └── ConfettiRoot     empty RectTransform  full-stretch  ← confettiRoot
///      │
///      └── DefeatPanel      GameObject, add CanvasGroup, start INACTIVE
///          ├── Background       Image  dark-red  alpha 0.90  full-stretch
///          ├── RedFlash         Image  red        alpha 0     full-stretch  ← defeatFlash
///          ├── TitleText        TMP_Text  "GAME OVER"  start INACTIVE       ← defeatTitle
///          └── DefeatBtnPanel   GameObject, add CanvasGroup, start INACTIVE  ← defeatButtonPanel
///              └── RetryBtn     Button → WinLoseUI.OnPlayAgain()
///
///  Drag all Inspector slots below, then set every panel INACTIVE in the hierarchy.
/// ─────────────────────────────────────────────────────────────────────────
/// </summary>
public class WinLoseUI : MonoBehaviour
{
    // ── Victory ───────────────────────────────────────────────────────────────
    [Header("── Victory ───────────────────────────────────")]
    public CanvasGroup victoryGroup;
    [Tooltip("Full-screen white Image (alpha 0 at start).")]
    public Image       victoryFlash;
    [Tooltip("'VICTORY!' TMP_Text (start inactive).")]
    public TMP_Text    victoryTitle;
    [Tooltip("CanvasGroup parent of the three score TMP texts (start inactive).")]
    public CanvasGroup scorePanel;
    public TMP_Text    perfectCountText;
    public TMP_Text    goodCountText;
    public TMP_Text    missCountText;
    [Tooltip("CanvasGroup parent of Play Again + Main Menu buttons (start inactive).")]
    public CanvasGroup buttonPanel;

    // ── Defeat ────────────────────────────────────────────────────────────────
    [Header("── Defeat ────────────────────────────────────")]
    public CanvasGroup defeatGroup;
    [Tooltip("Full-screen red Image (alpha 0 at start).")]
    public Image       defeatFlash;
    [Tooltip("'GAME OVER' TMP_Text (start inactive).")]
    public TMP_Text    defeatTitle;
    [Tooltip("CanvasGroup parent of the Retry button (start inactive).")]
    public CanvasGroup defeatButtonPanel;

    // ── Confetti ──────────────────────────────────────────────────────────────
    [Header("── Confetti ──────────────────────────────────")]
    [Tooltip("Empty full-stretch RectTransform inside VictoryPanel.")]
    public RectTransform confettiRoot;
    [Range(20, 80)]
    public int confettiCount = 48;

    private static readonly Color[] ConfettiColors =
    {
        new Color(1.00f, 0.84f, 0.00f),   // gold
        new Color(0.30f, 0.76f, 1.00f),   // sky blue
        new Color(0.40f, 1.00f, 0.40f),   // lime
        new Color(1.00f, 0.44f, 0.44f),   // coral
        new Color(1.00f, 0.55f, 0.00f),   // orange
        new Color(0.88f, 0.25f, 1.00f),   // violet
    };

    // ── Animators ────────────────────────────────────────────────────────────
    [Header("── Animators (optional) ──────────────────────")]
    public Animator characterAnimator;
    [Tooltip("Trigger name on character Animator for winning.")]
    public string   victoryTrigger   = "Victory";
    [Tooltip("Trigger name on character Animator for losing.")]
    public string   defeatTrigger    = "Defeat";

    public Animator enemyAnimator;
    [Tooltip("Trigger on enemy when player wins.")]
    public string   enemyFallTrigger = "Fall";
    [Tooltip("Trigger on enemy when player loses.")]
    public string   enemyWinTrigger  = "Win";

    // ── Scene Navigation ─────────────────────────────────────────────────────
    [Header("── Scene Navigation ─────────────────────────")]
    [Tooltip("Exact name of your main menu scene (must be in Build Settings).\n" +
             "Leave blank to load build index 0.")]
    public string mainMenuSceneName = "MainMenu";

    // ═════════════════════════════════════════════════════════════════════════
    void Awake()
    {
        // Start everything hidden and non-interactive
        GroupOff(victoryGroup);
        GroupOff(defeatGroup);
        GroupOff(scorePanel);
        GroupOff(buttonPanel);
        GroupOff(defeatButtonPanel);
        ZeroAlpha(victoryFlash);
        ZeroAlpha(defeatFlash);
    }

    // ── Called by FightProgress ───────────────────────────────────────────────

    public void ShowVictory(int perfects, int goods, int misses)
    {
        StartCoroutine(VictorySequence(perfects, goods, misses));
    }

    public void ShowDefeat()
    {
        StartCoroutine(DefeatSequence());
    }

    // ── Button callbacks (wire in Inspector) ──────────────────────────────────

    public void OnPlayAgain()
    {
        StartCoroutine(LoadByIndex(SceneManager.GetActiveScene().buildIndex));
    }

    public void OnMainMenu()
    {
        if (string.IsNullOrEmpty(mainMenuSceneName))
            StartCoroutine(LoadByIndex(0));
        else
            StartCoroutine(LoadByName(mainMenuSceneName));
    }

    // ═════════════════════════════════════════════════════════════════════════
    //  Victory Sequence
    // ═════════════════════════════════════════════════════════════════════════

    IEnumerator VictorySequence(int perfects, int goods, int misses)
    {
        // 1. White screen flash
        if (victoryFlash != null)
            yield return StartCoroutine(Flash(victoryFlash, Color.white, 0.90f, 0.35f));

        // 2. Fire character / enemy animations
        characterAnimator?.SetTrigger(victoryTrigger);
        enemyAnimator    ?.SetTrigger(enemyFallTrigger);

        // 3. Dark overlay fades in
        victoryGroup.gameObject.SetActive(true);
        yield return StartCoroutine(FadeGroup(victoryGroup, 0f, 1f, 0.45f));
        MakeInteractable(victoryGroup);

        // 4. "VICTORY!" title slams in with scale punch
        if (victoryTitle != null)
        {
            victoryTitle.gameObject.SetActive(true);
            victoryTitle.rectTransform.localScale = Vector3.zero;
            yield return StartCoroutine(PunchScale(victoryTitle.rectTransform,
                                                   peak: 1.55f, inDur: 0.13f, outDur: 0.10f));
        }

        yield return new WaitForSecondsRealtime(0.15f);

        // 5. Score panel appears, numbers count up
        if (scorePanel != null)
        {
            scorePanel.gameObject.SetActive(true);
            yield return StartCoroutine(FadeGroup(scorePanel, 0f, 1f, 0.30f));

            // Stagger each row
            yield return StartCoroutine(CountUp(perfectCountText, perfects,
                                                "Perfect", new Color(1f, 0.84f, 0f)));
            yield return StartCoroutine(CountUp(goodCountText,    goods,
                                                "Good",    Color.green));
            yield return StartCoroutine(CountUp(missCountText,    misses,
                                                "Miss",    Color.grey));
        }

        yield return new WaitForSecondsRealtime(0.20f);

        // 6. Buttons appear
        if (buttonPanel != null)
        {
            buttonPanel.gameObject.SetActive(true);
            yield return StartCoroutine(FadeGroup(buttonPanel, 0f, 1f, 0.30f));
            MakeInteractable(buttonPanel);
        }

        // 7. Confetti bursts from top
        if (confettiRoot != null)
            StartCoroutine(BurstConfetti());
    }

    // ═════════════════════════════════════════════════════════════════════════
    //  Defeat Sequence
    // ═════════════════════════════════════════════════════════════════════════

    IEnumerator DefeatSequence()
    {
        // 1. Red screen flash
        if (defeatFlash != null)
            yield return StartCoroutine(Flash(defeatFlash,
                                              new Color(0.75f, 0f, 0f), 0.85f, 0.50f));

        // 2. Fire animations
        characterAnimator?.SetTrigger(defeatTrigger);
        enemyAnimator    ?.SetTrigger(enemyWinTrigger);

        // 3. Dark overlay fades in
        defeatGroup.gameObject.SetActive(true);
        yield return StartCoroutine(FadeGroup(defeatGroup, 0f, 1f, 0.50f));
        MakeInteractable(defeatGroup);

        // 4. "GAME OVER" slams in then glitch-reveals
        if (defeatTitle != null)
        {
            defeatTitle.gameObject.SetActive(true);
            defeatTitle.rectTransform.localScale = Vector3.zero;
            yield return StartCoroutine(PunchScale(defeatTitle.rectTransform,
                                                   peak: 1.40f, inDur: 0.10f, outDur: 0.08f));
            yield return new WaitForSecondsRealtime(0.10f);
            yield return StartCoroutine(GlitchReveal(defeatTitle, "GAME OVER", 1.20f));
        }

        yield return new WaitForSecondsRealtime(0.15f);

        // 5. Retry button appears
        if (defeatButtonPanel != null)
        {
            defeatButtonPanel.gameObject.SetActive(true);
            yield return StartCoroutine(FadeGroup(defeatButtonPanel, 0f, 1f, 0.30f));
            MakeInteractable(defeatButtonPanel);
        }
    }

    // ═════════════════════════════════════════════════════════════════════════
    //  Reusable Animation Coroutines
    //  ALL use Time.unscaledDeltaTime so they run while timeScale = 0
    // ═════════════════════════════════════════════════════════════════════════

    IEnumerator FadeGroup(CanvasGroup g, float from, float to, float dur)
    {
        float t = 0f;
        g.alpha = from;
        while (t < dur)
        {
            t      += Time.unscaledDeltaTime;
            g.alpha = Mathf.Lerp(from, to, Mathf.Clamp01(t / dur));
            yield return null;
        }
        g.alpha = to;
    }

    IEnumerator Flash(Image img, Color col, float peakAlpha, float dur)
    {
        img.color = new Color(col.r, col.g, col.b, peakAlpha);
        img.gameObject.SetActive(true);

        float t = 0f;
        while (t < dur)
        {
            t      += Time.unscaledDeltaTime;
            float a  = Mathf.Lerp(peakAlpha, 0f, Mathf.Clamp01(t / dur));
            img.color = new Color(col.r, col.g, col.b, a);
            yield return null;
        }
        img.gameObject.SetActive(false);
    }

    IEnumerator PunchScale(RectTransform rt, float peak, float inDur, float outDur)
    {
        float t = 0f;
        while (t < inDur)
        {
            t += Time.unscaledDeltaTime;
            float s = Mathf.SmoothStep(0f, peak, Mathf.Clamp01(t / inDur));
            rt.localScale = new Vector3(s, s, 1f);
            yield return null;
        }
        t = 0f;
        while (t < outDur)
        {
            t += Time.unscaledDeltaTime;
            float s = Mathf.Lerp(peak, 1f, Mathf.Clamp01(t / outDur));
            rt.localScale = new Vector3(s, s, 1f);
            yield return null;
        }
        rt.localScale = Vector3.one;
    }

    IEnumerator CountUp(TMP_Text label, int target, string prefix, Color col)
    {
        if (label == null) yield break;
        label.color = col;
        const float dur = 0.65f;
        float t = 0f;
        while (t < dur)
        {
            t      += Time.unscaledDeltaTime;
            int val  = Mathf.RoundToInt(Mathf.Lerp(0, target, Mathf.Clamp01(t / dur)));
            label.text = $"{prefix}: {val}";
            yield return null;
        }
        label.text = $"{prefix}: {target}";
        yield return new WaitForSecondsRealtime(0.07f);
    }

    IEnumerator GlitchReveal(TMP_Text label, string finalText, float dur)
    {
        const string glyphPool   = "!@#$%^&*<>?/|~";
        const float  swapInterval = 0.055f;
        char[]       buf          = new char[finalText.Length];
        float t = 0f, nextSwap = 0f;

        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            if (t >= nextSwap)
            {
                nextSwap    = t + swapInterval;
                int revealed = Mathf.RoundToInt((t / dur) * finalText.Length);
                for (int i = 0; i < finalText.Length; i++)
                {
                    if (finalText[i] == ' ') { buf[i] = ' '; continue; }
                    buf[i] = i < revealed
                        ? finalText[i]
                        : glyphPool[Random.Range(0, glyphPool.Length)];
                }
                label.text = new string(buf);
            }
            yield return null;
        }
        label.text = finalText;
    }

    // ═════════════════════════════════════════════════════════════════════════
    //  Confetti
    // ═════════════════════════════════════════════════════════════════════════

    IEnumerator BurstConfetti()
    {
        Rect r = confettiRoot.rect;
        for (int i = 0; i < confettiCount; i++)
        {
            SpawnPiece(r);
            yield return new WaitForSecondsRealtime(0.035f);
        }
    }

    void SpawnPiece(Rect r)
    {
        var go  = new GameObject("Confetti");
        go.transform.SetParent(confettiRoot, false);

        var img           = go.AddComponent<Image>();
        img.color         = ConfettiColors[Random.Range(0, ConfettiColors.Length)];
        img.raycastTarget = false;

        var rt            = go.GetComponent<RectTransform>();
        float w           = Random.Range(7f, 14f);
        rt.sizeDelta      = new Vector2(w, w * (Random.value > 0.5f ? 1f : 1.9f));
        rt.anchoredPosition = new Vector2(
            Random.Range(r.xMin * 0.75f, r.xMax * 0.75f),
            r.yMax * 0.45f);
        rt.localRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

        StartCoroutine(DropPiece(rt, img));
    }

    IEnumerator DropPiece(RectTransform rt, Image img)
    {
        float fallSpeed = Random.Range(280f, 650f);
        float spin      = Random.Range(-280f, 280f);
        float swayFreq  = Random.Range(1.5f, 3.0f);
        float swayAmp   = Random.Range(30f, 80f);
        float dur       = Random.Range(1.8f, 3.4f);
        float delay     = Random.Range(0f, 0.55f);

        if (delay > 0f) yield return new WaitForSecondsRealtime(delay);

        float   t      = 0f;
        Vector2 origin = rt.anchoredPosition;
        Color   col    = img.color;

        while (rt != null && t < dur)
        {
            t += Time.unscaledDeltaTime;

            rt.anchoredPosition = new Vector2(
                origin.x + swayAmp * Mathf.Sin(t * swayFreq),
                origin.y - fallSpeed * t);

            rt.localRotation = Quaternion.Euler(0f, 0f, spin * t);

            // Fade out in the final 0.5 s
            col.a     = t > dur - 0.5f
                ? Mathf.Lerp(1f, 0f, (t - (dur - 0.5f)) / 0.5f)
                : 1f;
            img.color = col;

            yield return null;
        }

        if (rt != null) Destroy(rt.gameObject);
    }

    // ═════════════════════════════════════════════════════════════════════════
    //  Scene Loading  — MUST restore timeScale = 1 before switching
    // ═════════════════════════════════════════════════════════════════════════

    IEnumerator LoadByIndex(int index)
    {
        yield return new WaitForSecondsRealtime(0.35f);
        Time.timeScale = 1f;                   // ← critical: unfreeze before load
        SceneManager.LoadScene(index);
    }

    IEnumerator LoadByName(string sceneName)
    {
        yield return new WaitForSecondsRealtime(0.35f);
        Time.timeScale = 1f;                   // ← critical: unfreeze before load
        SceneManager.LoadScene(sceneName);
    }

    // ═════════════════════════════════════════════════════════════════════════
    //  Helpers
    // ═════════════════════════════════════════════════════════════════════════

    static void GroupOff(CanvasGroup g)
    {
        if (g == null) return;
        g.alpha = 0f; g.interactable = false; g.blocksRaycasts = false;
    }

    static void MakeInteractable(CanvasGroup g)
    {
        if (g == null) return;
        g.interactable = true; g.blocksRaycasts = true;
    }

    static void ZeroAlpha(Image img)
    {
        if (img == null) return;
        Color c = img.color; c.a = 0f; img.color = c;
    }
}