
using UnityEngine;

namespace Khoai
{
    public class KThemeManager : KSingleton<KThemeManager>
    {
        public KColorPalette colorPalette;

        protected override void Start()
        {
            base.Start();
            colorPalette = Resources.Load<KColorPalette>("ColorPallete");
        }

        private void NotifyKUIs()
        {
            GetGameObjectsByType<KIColoredUI>()
                .ForEach(k => (k as KIColoredUI).ColorsUpdated(colorPalette));
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            NotifyKUIs();
        }
#endif
    }
}