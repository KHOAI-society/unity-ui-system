using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Khoai
{

    [CreateAssetMenu(menuName = "UI/KSpritePallete")]
    public class KSpritePalette : ScriptableObject
    {
        [SerializeField] List<string> spriteNamesList;
        [SerializeField] List<Sprite> spriteList;
        public Dictionary<string, Sprite> SpriteMap
        {
            get
            {
                var result = new Dictionary<string, Sprite>();
                // Loop through the list of keys and values and add each key/value pair to the dictionary
                for (int i = 0; i != spriteNamesList.Count; i++)
                    result.Add(spriteNamesList[i], spriteList[i]);
                return result;
            }
        }

    #if UNITY_EDITOR
        private void OnValidate()
        {
            KTheme.Instance.UpdateSpriteAppliedObject();
        }
#endif
    }
}
