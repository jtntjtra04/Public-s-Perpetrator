using UnityEngine;
using UnityEngine.Rendering.Universal; // Essential for Light2D
using System.Collections;


public class FaintLight : MonoBehaviour
{
    [Header("URP 2D Light Reference")]
    [SerializeField] private Light2D faintLight;                // 2D spot light

    [Header("Intensity Settings")]
    [SerializeField] private float normalIntensity = 0.8f;      // initial value
    [SerializeField] private float targetIntensity = 2.0f;      // target value

    [Header("Independent Timing")]
    [SerializeField] private float pulseFrequency = 4.0f;        // delay
    [SerializeField] private float fadeDuration = 1.0f;         // transition time
    [SerializeField] private float faintDuration = 1.5f;        // faint light duration

    private bool headlampOn = true;
    private Coroutine lampLoop;
    AudioManager audio_manager;


    private void Start()
    {
        if (faintLight != null)
        {
            StartCoroutine(FaintLampLoop());
        }
        else
        {
            Debug.LogError($"PlayerLightPulse script on {gameObject.name} requires a Light2D component!");
        }
    }


    public void SetFaintState(bool activate)
    {
        headlampOn = activate;

        if (lampLoop != null)                                          // force disable any existing loop before exiting a room
        {
            StopCoroutine(lampLoop);
            lampLoop = null;
            Debug.Log("lamp force stopped");
        }

        if (!headlampOn)
        {
            faintLight.intensity = normalIntensity;                         // lock to normal room illumination instantly
            Debug.Log("lamp stopped");
        }
        else
        {
            faintLight.intensity = targetIntensity;
            lampLoop = StartCoroutine(FaintLampLoop());                 // lock clean state and fire up exactly ONE clean loop thread from baseline
            Debug.Log("lamp happening");
        }
    }


    private IEnumerator FaintLampLoop()
    {
        while (true)
        {
            if (!headlampOn) break;

            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                if (!headlampOn) break;
                elapsed += Time.deltaTime;
                faintLight.intensity = Mathf.Lerp(normalIntensity, targetIntensity, elapsed / fadeDuration);    // turn off
                yield return null;
            }
            if (!headlampOn) break;
            faintLight.intensity = targetIntensity;


            yield return new WaitForSeconds(faintDuration);                                                     // hold
            if (!headlampOn) break;


            elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                if (!headlampOn) break;
                elapsed += Time.deltaTime;
                faintLight.intensity = Mathf.Lerp(targetIntensity, normalIntensity, elapsed / fadeDuration);    // turn back on
                yield return null;
            }
            faintLight.intensity = normalIntensity;
            yield return new WaitForSeconds(pulseFrequency);                                                    // delay between blackouts
        }
    }
}
