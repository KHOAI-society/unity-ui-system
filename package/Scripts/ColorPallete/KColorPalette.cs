using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Khoai
{
    public class KColorSelectionAttribute : PropertyAttribute { }

    [CreateAssetMenu(menuName = "UI/KColorPalette")]
    public class KColorPalette : ScriptableObject
    {
        [SerializeField] List<string> dictsName;
        [SerializeField] List<Color> dictsColors;
        public Dictionary<string, Color> colorsList
        {
            get
            {
                var result = new Dictionary<string, Color>();
                // Loop through the list of keys and values and add each key/value pair to the dictionary
                for (int i = 0; i != dictsName.Count; i++)
                    result.Add(dictsName[i], dictsColors[i]);
                return result;
            }
        }

    #if UNITY_EDITOR
        private void OnValidate()
        {
            var coloredUIs = KMonoBehaviour.GetGameObjectsByType<KIColoredUI>();
            Debug.Log($"ui count: {coloredUIs.Count()}");
            foreach (var item in coloredUIs)
                (item as KIColoredUI).SyncColor(this);
        }
#endif
    }
}
