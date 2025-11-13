
using System;
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
            try
            {
                GetComponent<Image>().color = palette.colorsList[mainColor];
            }
            catch(Exception ex)
            {
                Debug.LogException(ex);
                GetComponent<Image>().color = errorColor;
            }
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            SyncColor();
        }
#endif
    }
}