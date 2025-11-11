
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Khoai
{
    public class KMonoBehaviour : MonoBehaviour
    {
        private static readonly List<GameObject> gameObjects = new();
        private static readonly Dictionary<Type, List<Component>> componentsCache = new();

        protected virtual void Start()
        {
            AddObjectToCache(gameObject);
        }

        protected virtual void OnDestroy()
        {
            RemoveObjectFromCache(gameObject);
        }

        public static List<Component> GetGameObjectsByType<T>()
        {
            return GetGameObjectsByType(typeof(T));
        }

        public static List<Component> GetGameObjectsByType(Type type)
        {
            if (componentsCache.ContainsKey(type)) return componentsCache[type];

            componentsCache[type] = new List<Component>();
            foreach (var go in gameObjects)
            {
                AddObjectToCache(go);
            }
            return componentsCache[type];
        }

        public static void AddObjectToCache(GameObject go)
        {
            var types = componentsCache.Keys;
            foreach (var type in types)
            { 
                var component = go.GetComponent(type);
                if (!component) continue;
                componentsCache[type].Add(component);
            }
        }

        public static void RemoveObjectFromCache(GameObject go)
        {
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
        }
    }
}