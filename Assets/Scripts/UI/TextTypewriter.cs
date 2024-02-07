using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextTypewriter : MonoBehaviour
{
    [Range(0.01f, 1f)]
    public float typingSpeed = 0.05f;
    public string fullText;
    private string currentText = "";

    private void Start()
    {
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        for (int i = 0; i < fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i + 1);
            GetComponent<TextMeshProUGUI>().text = currentText;

            yield return new WaitForSeconds(typingSpeed);
        }
    }

}
