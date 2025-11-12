using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Khoai
{
    public class KMonoBehaviour : MonoBehaviour
    {
#if !UNITY_EDITOR
        private static readonly List<GameObject> gameObjects = new();
        private static readonly Dictionary<Type, List<Component>> componentsCache = new();
#endif

        protected virtual void Start()
        {
#if !UNITY_EDITOR
            AddObjectToCache(gameObject);
#endif
        }

        protected virtual void OnDestroy()
        {
#if !UNITY_EDITOR
            RemoveObjectFromCache(gameObject);
#endif            
        }

        public static IEnumerable<MonoBehaviour> GetGameObjectsByType<T>()
        {
            Type type = typeof(T);
#if UNITY_EDITOR
            var components = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            return components.Where(c => c is T);
#else
            if (componentsCache.ContainsKey(type)) return componentsCache[type];

            componentsCache[type] = new List<Component>();
            foreach (var go in gameObjects)
            {
                AddObjectToCache(go);
            }
            return componentsCache[type];
#endif
        }

        public static void AddObjectToCache(GameObject go)
        {
#if !UNITY_EDITOR
            if (gameObjects.Contains(go)) return;
            gameObjects.Add(go);

            var types = componentsCache.Keys;
            foreach (var type in types)
            { 
                var component = go.GetComponent(type);
                if (!component) continue;
                componentsCache[type].Add(component);
            }
#endif
        }

        public static void RemoveObjectFromCache(GameObject go)
        {
#if !UNITY_EDITOR
            var types = componentsCache.Keys;
            foreach (var type in types)
            {
                foreach (var component in componentsCache[type])
                {
                    if (component.gameObject != go) continue;
                    componentsCache[type].Remove(component);
                    break;
                }
            }
#endif
        }
    }
}
