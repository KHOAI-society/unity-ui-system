using UnityEngine.UI;

namespace Khoai
{
    public class KSelectable : Selectable
    {
#if !UNITY_EDITOR
        private static readonly List<GameObject> gameObjects = new();
        private static readonly Dictionary<Type, List<Component>> componentsCache = new();
#endif

        protected override void Start()
        {
            base.Start();
#if !UNITY_EDITOR
            AddObjectToCache(gameObject);
#endif
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
#if !UNITY_EDITOR
            RemoveObjectFromCache(gameObject);
#endif            
        }
    }
}
