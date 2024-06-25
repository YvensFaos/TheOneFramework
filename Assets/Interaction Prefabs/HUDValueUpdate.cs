using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using TMPro;
using System.Text.RegularExpressions;

[RequireComponent(typeof(TMP_Text))]
public class HUDValueUpdate : MonoBehaviour
{
    public VariableStorageBehaviour variableStorageBehaviour;
    private TMP_Text targetText;
    private string originalText;
    private string lastKnownText;

    void Start() {
        if (variableStorageBehaviour == null) {
            Debug.LogWarning("VariableStorageBehaviour not defined. This will result in errors.");
        }
        targetText = GetComponent<TMP_Text>();
        originalText = targetText.text;
        lastKnownText = originalText;
    }

    private string GetValue(string key) {
        if (variableStorageBehaviour.TryGetValue<object>(key, out var value)) {
            return string.Format("{0}", value.ToString());
        } else {
            return "<unknown value>";
        }
    }
    string UpdateValuesInString(string value) {
        string pattern = @"\{\$?([a-zA-Z_][a-zA-Z0-9_]*)\}";
        Regex regex = new Regex(pattern);

        value = regex.Replace(value, match => {
            string key = "$" + match.Groups[1].Value;
            string replacementValue = GetValue(key);
            return replacementValue;
        });

        return value;
    }

    void LateUpdate()
    {
        if (targetText != null) {
            if (targetText.text != lastKnownText) {
                lastKnownText = targetText.text;
                originalText = lastKnownText;
            }

            targetText.text = UpdateValuesInString(originalText);
            lastKnownText = targetText.text;
        }
    }
}
