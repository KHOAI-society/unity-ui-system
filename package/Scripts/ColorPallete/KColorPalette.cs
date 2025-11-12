using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Khoai
{
    public class KColorList : PropertyAttribute { }

    [CreateAssetMenu(menuName = "UI/KColorPalette")]
    public class KColorPalette : ScriptableObject
    {
        public event Action Changed;

        public List<Color> colors = new();

    #if UNITY_EDITOR
        private void OnValidate()
        {
            var coloredUIs = KMonoBehaviour.GetGameObjectsByType<KIColoredUI>();
            Debug.Log($"ui count: {coloredUIs.Count()}");
            foreach(var item in coloredUIs)
                (item as KIColoredUI).SyncColor(this);
        }
    #endif
    }
}
