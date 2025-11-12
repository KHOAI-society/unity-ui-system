using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Khoai
{
    public class KColorSelectionAttribute : PropertyAttribute { }

    [CreateAssetMenu(menuName = "UI/KColorPalette")]
    public class KColorPalette : ScriptableObject
    {
        public Dictionary<string, Color> colorsList;

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
