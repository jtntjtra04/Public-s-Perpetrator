using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;


public class AmbientDarkness : MonoBehaviour
{
    [Header("URP 2D Light Reference")]
    [SerializeField] private Light2D globalLight;                   // 2D global light

    [Header("Pulse Settings")]
    [SerializeField] private float pulseFrequency = 3.0f;           // lit duration
    [SerializeField] private float transitionSpeed = 0.6f;          // stutters
    [SerializeField] private float darkDuration = 2.0f;             // blackout duration

    [Header("Color States")]
    [SerializeField] private Color normalTint = new Color(0.35f, 0.35f, 0.4f, 1f);
    [SerializeField] private Color darkTint = Color.black;

    private bool blackoutActive = true;
    private Coroutine darkLoop;
    AudioManager audio_manager;


    private void Start()
    {
        if (globalLight != null)
        {
            StartCoroutine(BlackoutLoop());
        }
        else
        {
            Debug.LogError("AmbientDarknessPulse requires a 2D Global Light to operate!");
        }
    }


    private void Awake()
    {
        audio_manager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }


    public void SetBlackoutState(bool activate)
    {
        blackoutActive = activate;

        if (darkLoop != null)                                       // force disable any existing loop before exiting a room
        {
            StopCoroutine(darkLoop);
            darkLoop = null;
            Debug.Log("blackout force stopped");
        }

        if (!blackoutActive)
        {
            globalLight.color = normalTint;                         // lock to normal room illumination instantly
            Debug.Log("blackout stopped");
        }
        else
        {
            globalLight.color = normalTint;
            darkLoop = StartCoroutine(BlackoutLoop());              // lock clean state and fire up exactly ONE clean loop thread from baseline
            Debug.Log("blackout happening");
        }
    }


    private IEnumerator BlackoutLoop()
    {
        while (blackoutActive)
        {
            if (!blackoutActive) break;

            audio_manager.PlaySFX(audio_manager.electricity, 1f, 0.35f);
            float elapsed = 0f;
            while (elapsed < transitionSpeed)
            {
                if (!blackoutActive) break;
                elapsed += Time.deltaTime;
                globalLight.color = Random.Range(0f, 1f) > 0.4f ? darkTint : normalTint;            // stutter to dark
                yield return null;
            }
            if (!blackoutActive) break;
            globalLight.color = darkTint;


            yield return new WaitForSeconds(darkDuration);                                          // how long the blackout remains
            if (!blackoutActive) break;


            audio_manager.PlaySFX(audio_manager.electricity, 2f, 0.35f);
            elapsed = 0f;
            while (elapsed < transitionSpeed)
            {
                if (!blackoutActive) break;
                elapsed += Time.deltaTime;
                globalLight.color = Random.Range(0f, 1f) > 0.5f ? normalTint : darkTint;            // stutter to light
                yield return null;
            }
            globalLight.color = normalTint;
            yield return new WaitForSeconds(pulseFrequency);                                        // delay between blackouts
        }
    }
}
