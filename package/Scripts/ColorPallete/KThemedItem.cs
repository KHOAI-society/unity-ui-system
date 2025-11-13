using System.Drawing;

namespace Khoai
{
    public class KThemedItem : KMonoBehaviour
    {
        public static readonly UnityEngine.Color errorColor = new(1f, 0f, 1f);
        public KColorPalette palette;

        protected override void Start()
        {
            base.Start();
            SyncColor(KTheme.Instance.colorPalette);
        }

        public virtual void SyncColor()
        {
            SyncColor(palette);
        }

        public virtual void SyncColor(KColorPalette colorPalette)
        {
            palette = colorPalette;
        }
    }
}