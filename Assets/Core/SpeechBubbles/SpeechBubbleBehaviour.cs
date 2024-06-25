using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechBubbleBehaviour : MonoBehaviour
{    
    const float MinWidth = 0.1f;
    const float MaxWidth = 2.0f;
    const float MinHeight = 0.12f;
    const float MaxHeight = 1.5f;

    private float borderSize = 0.05f;
    private float tailSize = 0.05f;

    void Start() {
        //borderSize = 
    }

    public float GetHeight() {
        var bodyNode = transform.Find("Body");
        //0.02=border 0.12=tail size
        return bodyNode.GetComponent<RectTransform>().sizeDelta.y + borderSize + tailSize;
    }

    public void SetMessage(string text) {
        var bodyNode = transform.Find("Body");
        var textNode = bodyNode.Find("Text");
        var textMeshPro = textNode.GetComponent<TextMeshPro>(); 

        Vector2 preferredValues = textMeshPro.GetPreferredValues(text, MaxWidth, float.MaxValue);
        float width = Mathf.Min(MaxWidth, preferredValues.x);
        float height = preferredValues.y;
        if (width < MinWidth) width = MinWidth;
        if (height < MinHeight) height = MinHeight;
        if (height > MaxHeight) height = MaxHeight;
        bodyNode.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        textMeshPro.text = text;
    }
}
