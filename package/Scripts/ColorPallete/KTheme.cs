using System.Collections.Generic;
using UnityEngine;

namespace Khoai
{
    [CreateAssetMenu(menuName = "UI/KTheme")]
    [ExecuteAlways]
    public partial class KTheme : ScriptableObject
    {
        public string version;
        public KColorPalette colorPalette;
        List<Component> coloredUIs;

        void OnEnable()
        {
            coloredUIs = KMonoBehaviour.GetGameObjectsByType<KIColoredUI>();
            coloredUIs?.ForEach(k => (k as KIColoredUI).ColorsUpdated(colorPalette));
        }

        void OnDestroy()
        {
            coloredUIs = null;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            coloredUIs?.ForEach(k => (k as KIColoredUI).ColorsUpdated(colorPalette));
        }
    #endif
    }
}