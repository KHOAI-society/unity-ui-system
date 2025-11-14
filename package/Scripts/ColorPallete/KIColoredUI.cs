
using UnityEngine;

namespace Khoai
{
    public class KColorSelectionAttribute : PropertyAttribute { }
    public class KReadOnlyAttribute : PropertyAttribute { }
    public interface KIColoredUI
    {
        public void SyncColor();
        public void SyncColor(KColorPalette colorPalette);

        public void SyncTexture2D();
        public void SyncTexture2D();
    }
}