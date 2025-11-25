
using UnityEngine;
using UnityEngine.UI;

namespace Khoai
{
    [RequireComponent(typeof(Text))]
    public class KThemedText : KThemedItem, KIColorAppliedItem
    {
        [KColorSelection] public KThemedItemProperty mainColor;

        public override void SyncColor(KColorPalette colorPalette)
        {
            base.SyncColor(colorPalette);
            if (!base.colorPalette) return;
            if(!mainColor.use) return;
            GetComponent<Text>().color = GetColor(mainColor.itemName);
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            SyncColor();
        }
#endif
    }
}