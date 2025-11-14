using System.Linq;
using UnityEngine;

namespace Khoai
{
    [CreateAssetMenu(menuName = "UI/KTheme")]
    public partial class KTheme : ScriptableObject
    {
        public KColorPalette colorPalette;
        public KTexture2DPallete texture2DPalette;

        public void UpdateColoredObject()
        {
            var coloredUIs = KMonoBehaviour.GetGameObjectsByType<KIColoredItem>();
            foreach(var item in coloredUIs)
                (item as KIColoredItem).SyncColor(colorPalette);
        }

        public void UpdateTexture2DAppliedObject()
        {
            var coloredUIs = KMonoBehaviour.GetGameObjectsByType<KITexture2DAppliedItem>();
            foreach(var item in coloredUIs)
                (item as KITexture2DAppliedItem).SyncTexture2D(texture2DPalette);
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            UpdateColoredObject();
            UpdateTexture2DAppliedObject();
        }
#endif
    }
}