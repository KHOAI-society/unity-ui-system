using System;
using UnityEngine;


namespace Khoai
{
    [CreateAssetMenu(menuName = "UI/KTheme")]
    public class KTheme : ScriptableObject
    {
        public KColorPalette colorPalette;
        public KSpritePalette spritePalette;
        private static KTheme _instance;
        public static KTheme Instance
        {
            get
            {
                if (_instance != null) return _instance;

                var resources = Resources.LoadAll<KTheme>("");
                if (resources.Length == 0)
                {
                    throw new Exception("There are no SriptableObject KTheme in Resources folder");
                }
                else if (resources.Length > 1)
                {
                    string message = "There more than 1 SriptableObject KTheme in Resources folder\n";
                    foreach (var res in resources)
                    {
                        message += $"{res.name} \n";
                    }
                    throw new Exception(message);
                }

                _instance = resources[0];
                _instance.UpdateColoredObject();
                _instance.UpdateSpriteAppliedObject();

                return _instance;
            }
        }

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