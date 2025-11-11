
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
    }
}