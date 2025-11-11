
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace Khoai
{
    [ExecuteAlways]
    public class KImage : Image, KIColoredUI
    {
        public KColorPalette palette;
        [KColorSelector] public int selectedColor;

        protected override void Awake()
        {
            base.Awake();
            KMonoBehaviour.AddObjectToCache(gameObject);
            palette = KThemeManager.Instance.colorPalette;
            UpdateColor();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            KMonoBehaviour.RemoveObjectFromCache(gameObject);
        }

        void UpdateColor()
        {
            if (!palette) return;
            color = palette.colors[selectedColor];
        }

        public void ColorsUpdated(KColorPalette colorPalette)
        {
            palette = colorPalette;
            UpdateColor();
        }
    }
}