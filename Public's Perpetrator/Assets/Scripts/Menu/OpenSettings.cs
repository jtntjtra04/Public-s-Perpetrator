using UnityEngine;


public class OpenSettings : MonoBehaviour
{
    public GameObject settingsPanel;


    public void OpenSettingsPanel()
    {
        settingsPanel.SetActive(true);
        Time.timeScale = 0f;
    }


    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
