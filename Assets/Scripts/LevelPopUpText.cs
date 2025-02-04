using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;


public class LevelPopUpText : MonoBehaviour
{
    [Header("Text Reference")]
    [Tooltip("Assign the TextMeshProUGUI component here.")]
    public TextMeshProUGUI tmpText;

    [Header("Scaling Settings")]
    [Tooltip("Maximum scale multiplier (e.g., 1.5 for 150%).")]
    public float maxScaleMultiplier = 1.5f;
    [Tooltip("Duration to scale up (in seconds).")]
    public float scaleUpDuration = 2f;
    [Tooltip("Duration to scale down (in seconds).")]
    public float scaleDownDuration = 2f;

    [Header("Transparency Settings")]
    [Tooltip("Initial alpha value (0 for fully transparent).")]
    [Range(0f, 1f)]
    public float initialAlpha = 0f;
    [Tooltip("Final alpha value (1 for fully opaque).")]
    [Range(0f, 1f)]
    public float finalAlpha = 1f;

    [Header("Wait Settings")]
    [Tooltip("Time to wait after scaling up and fading in before scaling down and fading out (in seconds).")]
    public float waitAfterFadeIn = 1f;

    [Header("Post Animation Settings")]
    [Tooltip("Choose what happens after the animation completes.")]
    public PostAnimationAction postAnimationAction = PostAnimationAction.Disable;

    public AnimationCurve fadeInCurve;

    public AnimationCurve fadeOutCurve;

    // Enum to define post-animation actions
    public enum PostAnimationAction
    {
        Disable,    // Disable the GameObject
        Destroy,    // Destroy the GameObject
        DoNothing   // Keep the GameObject active
    }

    // Private variables to store initial scale and color
    private Vector3 initialScale;
    private Color initialColor;

    void Awake()
    {
        // Ensure the TextMeshProUGUI component is assigned
        if (tmpText == null)
        {
            tmpText = GetComponent<TextMeshProUGUI>();
            if (tmpText == null)
            {
                Debug.LogError("TextPulseEffect: No TextMeshProUGUI component assigned or found on the GameObject.");
                enabled = false;
                return;
            }
        }

        // Store the initial scale
        initialScale = transform.localScale;

        // Store and modify the initial color's alpha
        initialColor = tmpText.color;
        Color modifiedColor = initialColor;
        modifiedColor.a = initialAlpha;
        tmpText.color = modifiedColor;

        // Set the initial scale
        transform.localScale = initialScale;
    }

    void Start()
    {
        // Start the pulse and fade animation
        StartCoroutine(PulseAndFade());
    }

    /// <summary>
    /// Coroutine to handle the pulse (scaling) and fade (transparency) animation sequence.
    /// </summary>
    /// <returns></returns>
    IEnumerator PulseAndFade()
    {
        // Fade In while Scaling Up
        yield return StartCoroutine(FadeAndScale(initialAlpha, finalAlpha, initialScale, initialScale * maxScaleMultiplier, scaleUpDuration, fadeInCurve));

        // Wait after Fade In and Scale Up
        yield return new WaitForSeconds(waitAfterFadeIn);

        // Fade Out while Scaling Down
        yield return StartCoroutine(FadeAndScale(finalAlpha, initialAlpha, transform.localScale, initialScale, scaleDownDuration, fadeOutCurve));

        // Post Animation Action
        switch (postAnimationAction)
        {
            case PostAnimationAction.Disable:
                gameObject.SetActive(false);
                break;
            case PostAnimationAction.Destroy:
                Destroy(gameObject);
                break;
            case PostAnimationAction.DoNothing:
                // Keep the GameObject active
                break;
        }
    }

    /// <summary>
    /// Coroutine to handle simultaneous fading and scaling over a duration.
    /// </summary>
    /// <param name="startAlpha">Starting alpha value.</param>
    /// <param name="endAlpha">Ending alpha value.</param>
    /// <param name="startScale">Starting scale vector.</param>
    /// <param name="endScale">Ending scale vector.</param>
    /// <param name="duration">Duration of the transition in seconds.</param>
    /// <returns></returns>
    IEnumerator FadeAndScale(float startAlpha, float endAlpha, Vector3 startScale, Vector3 endScale, float duration, AnimationCurve curve)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Calculate interpolation factor
            float t = Mathf.Clamp01(elapsed / duration);

            // Interpolate scale
            Vector3 currentScale = Vector3.Lerp(startScale, endScale, curve.Evaluate(t));
            transform.localScale = currentScale;

            // Interpolate alpha
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, curve.Evaluate(t));
            Color currentColor = tmpText.color;
            currentColor.a = currentAlpha;
            tmpText.color = currentColor;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure final scale and alpha are set
        transform.localScale = endScale;
        Color finalColor = tmpText.color;
        finalColor.a = endAlpha;
        tmpText.color = finalColor;
    }
}

