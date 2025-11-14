
using UnityEngine;

namespace Khoai
{
    public class KReadOnlyAttribute : PropertyAttribute { }
    public class KColorSelectionAttribute : PropertyAttribute { }
    public interface KIColorAppliedItem
    {
        public void SyncColor();
        public void SyncColor(KColorPalette colorPalette);
    }

    public class KSpriteSelectionAttribute : PropertyAttribute { }
    public interface KISpriteAppliedItem
    {
        public void SyncSprite();
        public void SyncSprite(KSpritePalette spritePalette);
    }
}