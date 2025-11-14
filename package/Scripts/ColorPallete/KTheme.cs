using System.Linq;
using UnityEngine;

namespace Khoai
{
    [CreateAssetMenu(menuName = "UI/KTheme")]
    public partial class KTheme : ScriptableObject
    {
        public KColorPalette colorPalette;

        public void UpdateColoredObject()
        {
            var coloredUIs = KMonoBehaviour.GetGameObjectsByType<KIColoredItem>();
            Debug.Log($"ui count: {coloredUIs.Count()}");
            foreach(var item in coloredUIs)
                (item as KIColoredItem).SyncColor(colorPalette);
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            UpdateColoredObject();
        }
#endif
    }
}