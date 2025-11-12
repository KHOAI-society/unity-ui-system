
using UnityEngine;
using UnityEngine.UI;

namespace Khoai
{
    [RequireComponent(typeof(Image))]
    public class KImage : KThemedItem, KIColoredUI
    {
        public override void SyncColor(KColorPalette colorPalette)
        {
            palette = colorPalette;

            if (!palette) return;
            if (palette.colors.Count == 0) return;
            GetComponent<Image>().color = palette.colors[selectedColor];
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            SyncColor();
        }
#endif
    }
}