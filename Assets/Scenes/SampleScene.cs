using System.Collections.Generic;
using Khoai;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SampleScene : MonoBehaviour
{
    [SerializeField] KColorPalette[] colorPalettes;
    [SerializeField] KSpritePalette[] spritePalettes;

    TextMeshProUGUI text;
    
    void Awake()
    {
        var dropdown = GameObject.Find("Canvas/Dropdown").GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();
        var options = new List<TMP_Dropdown.OptionData>();
        for(int i=0;i<2;i++)
        {
            options.Add(new TMP_Dropdown.OptionData
            {
                text = $"Color palette {i}"
            });
        }
        dropdown.AddOptions(options);
        dropdown.onValueChanged.AddListener(OnColorPaletteChanged);
        
        dropdown = GameObject.Find("Canvas/Dropdown (1)").GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();
        options = new List<TMP_Dropdown.OptionData>();
        for(int i=0;i<2;i++)
        {
            options.Add(new TMP_Dropdown.OptionData
            {
                text = $"Color sprite {i}"
            });
        }
        dropdown.AddOptions(options);
        dropdown.onValueChanged.AddListener(OnSpritePaletteChanged);
    }

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
        var color = text.color;
        color.r = red;
        text.color = color;
        KBindableContext.text.Value = $"{red}";
    }

    void OnTextChanged(string value)
    {
        text.text = value;
    }

    void OnAlphaChanged(float value)
    {
        var color = text.color;
        color.a = value;
        text.color = color;
        GameObject.Find("Canvas/Button").GetComponent<Image>().color = new Color(1, 1, 1, (Mathf.Sin(value) + 1) / 2);
    }

    void OnColorPaletteChanged(int id)
    {
        KTheme.Instance.SetColorPalette(colorPalettes[id]);
    } 

    void OnSpritePaletteChanged(int id)
    {
        KTheme.Instance.SetSpritePalette(spritePalettes[id]);
    } 
}
