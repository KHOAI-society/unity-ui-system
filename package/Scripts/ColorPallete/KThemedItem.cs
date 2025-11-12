
using UnityEngine;

namespace Khoai
{
    [ExecuteAlways]
    public class KThemedItem : KMonoBehaviour
    {
        public KColorPalette palette;
        [KColorSelector] public int selectedColor;

        protected override void Start()
        {
            base.Start();
            ColorsUpdated(KTheme.Instance.colorPalette);
        }

        public virtual void ColorsUpdated(KColorPalette colorPalette)
        {
            palette = colorPalette;
        }
    }
}