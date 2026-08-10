using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EndCutscene : MonoBehaviour
{
    public void GoToEnding()
    {
        SceneManager.LoadScene("Ch 1 Ending");
    }
}
