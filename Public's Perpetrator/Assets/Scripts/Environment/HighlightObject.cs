using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HighlightObject : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Header("Settings")]
    public Color highlightColor = Color.yellow;
    public float highlightDuration = 0.6f;                          // highlight duration
    public float fadeSpeed = 0.5f;                                  // fadeout duration

    private Color originalColor;
    private Coroutine activeTimerCoroutine;

    [Header("State")]
    public bool is_interacted = false;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }


    public void ShineLight()
    {
        if (is_interacted == true) return;                                  // important objects that have been interacted with no longer highlight

        spriteRenderer.color = highlightColor;                              // highlighted color

        activeTimerCoroutine = StartCoroutine(HighlightTimer());            // timer after it's highlighted
    }


    private IEnumerator HighlightTimer()
    {
        yield return new WaitForSeconds(highlightDuration);

        float elapsedTime = 0f;
        Color highlightedColor = spriteRenderer.color;

        while (elapsedTime < fadeSpeed)
        {
            elapsedTime += Time.deltaTime;
            float normalizedProgress = elapsedTime / fadeSpeed;

            spriteRenderer.color = Color.Lerp(highlightedColor, originalColor, normalizedProgress);    // blend colors as it starts from highlighted to original
            yield return null;
        }

        spriteRenderer.color = originalColor;
        activeTimerCoroutine = null;
    }
}


