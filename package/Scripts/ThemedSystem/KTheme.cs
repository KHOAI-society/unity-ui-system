using System.Linq;
using UnityEngine;

namespace Khoai
{
    [CreateAssetMenu(menuName = "UI/KTheme")]
    public partial class KTheme : ScriptableObject
    {
        public KColorPalette colorPalette;
        public KSpritePalette spritePalette;

        public void UpdateColoredObject()
        {
            var coloredUIs = KMonoBehaviour.GetGameObjectsByType<KIColorAppliedItem>();
            foreach(var item in coloredUIs)
                (item as KIColorAppliedItem).SyncColor(colorPalette);
        }

        public void UpdateSpriteAppliedObject()
        {
            var coloredUIs = KMonoBehaviour.GetGameObjectsByType<KISpriteAppliedItem>();
            foreach(var item in coloredUIs)
                (item as KISpriteAppliedItem).SyncSprite(spritePalette);
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            UpdateColoredObject();
            UpdateSpriteAppliedObject();
        }
#endif
    }
}