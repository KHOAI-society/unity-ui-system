using UnityEngine;
using System;
using System.Collections.Generic;

public enum ColorKey
{
    Primary,
    Secondary,
    Warning,
    Error,
    Empty,
    ABC
}

[CreateAssetMenu(menuName = "UI/ColorPalette")]
public class ColorPalette : ScriptableObject
{
    public event Action Changed;

    public List<Color> colors = new();

#if UNITY_EDITOR
    private void OnValidate()
    {
        int enumCount = Enum.GetValues(typeof(ColorKey)).Length;

        if (colors.Count < enumCount)
            colors.AddRange(new Color[enumCount - colors.Count]);
        else if (colors.Count > enumCount)
            colors.RemoveRange(enumCount, colors.Count - enumCount);

        Changed?.Invoke();
    }
#endif

    public Color KeyToColor(ColorKey key)
    {
        return colors[(int)key];
    }
}
