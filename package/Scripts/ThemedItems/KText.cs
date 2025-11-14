
using UnityEngine;
using UnityEngine.UI;

namespace Khoai
{
    [RequireComponent(typeof(Text))]
    public class KText : KThemedItem, KIColorAppliedItem
    {
        [KColorSelection]
        [SerializeField]
        KThemedItemProperty mainColor;

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