using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private float type_speed = 50f;


    public Coroutine Run(string textToType, TMP_Text text_Label)
    {
        return StartCoroutine(TypeText(textToType, text_Label));
    }


    private IEnumerator TypeText(string textToType, TMP_Text text_Label)
    {
        text_Label.text = string.Empty;     // empty box before typing
        float t = 0;                        // elapse time
        int charIndex = 0;                  // floored value of t

        while(charIndex < textToType.Length)
        {
            t += Time.deltaTime * type_speed;                           // time elapsed
            charIndex = Mathf.FloorToInt(t);                            // floored value of time elapsed
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);   
            text_Label.text = textToType.Substring(0, charIndex);       // write text
            yield return null;
        }

        text_Label.text = textToType;
    }
}
