using UnityEngine;
using System;
using System.Collections.Generic;



[CreateAssetMenu(menuName = "UI/ColorPalette")]
public class ColorPalette : ScriptableObject
{
    public event Action Changed;

    public List<Color> colors = new();

#if UNITY_EDITOR
    private void OnValidate()
    {
        Changed?.Invoke();
    }
#endif
}
