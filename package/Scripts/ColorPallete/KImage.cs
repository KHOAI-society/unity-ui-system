
using UnityEngine;
using UnityEngine.UI;

namespace Khoai
{
    [RequireComponent(typeof(Image))]
    public class KImage : KThemedItem, KIColoredUI
    {
        [KColorSelection]
        [SerializeField]
        string mainColor;

        public override void SyncColor(KColorPalette colorPalette)
        {
            base.SyncColor(colorPalette);
            if (!palette) return;
            if (palette.colorsList == null) 
            {
                GetComponent<Image>().color = errorColor;
                return;
            }
            if (!palette.colorsList.ContainsKey(mainColor))
            {
                GetComponent<Image>().color = errorColor;
                return;
            }
            GetComponent<Image>().color = palette.colorsList[mainColor];
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            SyncColor();
        }
#endif
    }
}