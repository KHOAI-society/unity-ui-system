namespace Khoai
{
    public class KThemedItem : KMonoBehaviour
    {
        public KColorPalette palette;
        public int selectedColor = 0;

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