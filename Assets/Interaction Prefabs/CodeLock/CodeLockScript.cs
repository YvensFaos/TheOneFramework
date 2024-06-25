using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Text;

public class CodeLockScript : MonoBehaviour
{
    public UnityEvent<string> onRightAnswer;
    public UnityEvent<string> onWrongAnswer;

    public int CodeLength = 4;

    public string CorrectCode = "1234";

    TextMeshProUGUI displayText; 

    [HideInInspector]
    public string code = "";

    void Start()
    {
        displayText = transform.Find("Display").GetComponentInChildren<TextMeshProUGUI>();

        Button[] buttons = GetComponentsInChildren<Button>();

        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => OnButtonClick(
                button.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
            ));
        }
    }

    void OnButtonClick(string name) {
        if (name == "<") {
            if (code.Length > 0) {
                code = code.Substring(0, code.Length - 1);
                UpdateDisplay();
            }
        }
        else if (name == "Ok") {
            CloseCodeLock();
            HandleCallback((code == CorrectCode), code);            
        }
        else if (name == "Close") {
            CloseCodeLock();
        }
        else if (code.Length < CodeLength) {
            code = code + name;
            UpdateDisplay();
        }
    }

    /* IEnumerator*/
    void HandleCallback(bool answerWasRight, string code) {
        //yield return null;
        if (answerWasRight) {
            onRightAnswer.Invoke(code);
        } else {
            onWrongAnswer.Invoke(code);
        }
    }

    void CloseCodeLock() {
        Canvas parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas != null) {
            parentCanvas.gameObject.SetActive(false);
        }
    }

    void UpdateDisplay() {
        string paddedCode = code.PadRight(CodeLength, '_');

        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < paddedCode.Length; i++) {
            stringBuilder.Append(paddedCode[i]);
            if (i != paddedCode.Length - 1)
                stringBuilder.Append(' ');
        }

        displayText.text = stringBuilder.ToString();
    }
}
