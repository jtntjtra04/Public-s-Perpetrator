using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class ParticipantIDController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_InputField idInputField;

    public Animator transition_fade;
    public SceneAudioManager menu_audio;


    public void ConfirmAndStartGame()
    {
        string enteredText = idInputField.text;

        if (string.IsNullOrWhiteSpace(enteredText))
        {
            AnalyticsManager.participant_id = "Unknown_User";
        }
        else
        {
            AnalyticsManager.participant_id = enteredText.Trim();   // pass into AnalyticsManager
        }

        AnalyticsManager.participant_id = $"{enteredText}_{Guid.NewGuid().ToString("N").Substring(0, 5)}";
        Debug.Log($"Participant ID Registered: {AnalyticsManager.participant_id}");
        StartCoroutine(ToMainMenu());
    }


    private IEnumerator ToMainMenu()      // fade transition and scene switching
    {
        transition_fade.enabled = true;
        transition_fade.Play("StartFade");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Main Menu"); ;
        transition_fade.Play("EndFade");
    }
}
