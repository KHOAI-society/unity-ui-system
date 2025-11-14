using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Khoai
{

    [CreateAssetMenu(menuName = "UI/KTexture2DPallete")]
    public class KTexture2DPallete : ScriptableObject
    {
        [SerializeField] List<string> colorNamesList;
        [SerializeField] List<Texture2D> colorList;
        public Dictionary<string, Texture2D> Texture2DMap
        {
            get
            {
                var result = new Dictionary<string, Texture2D>();
                // Loop through the list of keys and values and add each key/value pair to the dictionary
                for (int i = 0; i != colorNamesList.Count; i++)
                    result.Add(colorNamesList[i], colorList[i]);
                return result;
            }
        }

    #if UNITY_EDITOR
        private void OnValidate()
        {
            var coloredUIs = KMonoBehaviour.GetGameObjectsByType<KITexture2DAppliedItem>();
            foreach (var item in coloredUIs)
                (item as KITexture2DAppliedItem).SyncTexture2D(this);
        }
#endif
    }
}
