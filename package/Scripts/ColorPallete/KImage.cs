
using UnityEngine;
using UnityEngine.UI;

namespace Khoai
{
    [RequireComponent(typeof(Image))]
    [ExecuteAlways]
    public class KImage : KThemedItem, KIColoredUI
    {
        void UpdateColor()
        {
            if (!palette) return;
            GetComponent<Image>().color = palette.colors[selectedColor];
        }

        public override void ColorsUpdated(KColorPalette colorPalette)
        {
            UpdateColor();
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            UpdateColor();
        }
#endif
    }
}