
using UnityEngine;

namespace Khoai
{
    public class KReadOnlyAttribute : PropertyAttribute { }
    public class KColorSelectionAttribute : PropertyAttribute { }
    public interface KIColoredItem
    {
        public void SyncColor();
        public void SyncColor(KColorPalette colorPalette);
    }

    public class KTexture2DSelectionAttribute : PropertyAttribute { }
    public interface KITexture2DAppliedItem
    {
        public void SyncTexture2D();
        public void SyncTexture2D(KTexture2DPallete texture2DPallete);
    }
}