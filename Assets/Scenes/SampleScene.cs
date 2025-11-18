using System;
using Khoai;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SampleScene : MonoBehaviour
{
    TMPro.TextMeshProUGUI text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GameObject.Find("Canvas/Button/Text (TMP)").GetComponent<TextMeshProUGUI>();

        KBindableContext.text.RegisterListerner(OnTextChanged);
        KBindableContext.red.RegisterListerner(OnRedChanged);
        KBindableContext.alpha.RegisterListerner(OnAlphaChanged);
        GameObject.Find("Canvas/Button")
                .GetComponent<Button>()
                .onClick
                .AddListener(OnClick);
    }

    void Update()
    {
        KBindableContext.alpha.Value += Time.deltaTime;
    }

    void OnDestroy()
    {
        GameObject.Find("Canvas/Button")
                .GetComponent<Button>()
                .onClick
                .RemoveAllListeners();
        KBindableContext.text.DeregisterListener(OnTextChanged);
        KBindableContext.alpha.DeregisterListener(OnAlphaChanged);
        KBindableContext.red.DeregisterListener(OnRedChanged);
    }

    void OnClick()
    {
        var temp = KBindableContext.red.Value+1;
        KBindableContext.red.Value = temp;
    }

    void OnRedChanged(int red)
    {
        KBindableContext.text.Value = $"{red}";
    }

    void OnTextChanged(string value)
    {
        text.text = $"{value} {KBindableContext.red.Value} ";
    }

    void OnAlphaChanged(float value)
    {
        GameObject.Find("Canvas/Button").GetComponent<Image>().color = new Color(1, 1, 1, Mathf.Sin(value));
    }
}
