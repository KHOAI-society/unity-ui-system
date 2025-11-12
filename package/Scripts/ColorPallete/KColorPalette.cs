using UnityEngine;
using System;
using System.Collections.Generic;

namespace Khoai
{
    [CreateAssetMenu(menuName = "UI/KColorPalette")]
    public class KColorPalette : ScriptableObject
    {
        public event Action Changed;

        public List<Color> colors = new();

    #if UNITY_EDITOR
        private void OnValidate()
        {
            Changed?.Invoke();
            KMonoBehaviour.GetGameObjectsByType<KIColoredUI>()
            .ForEach(k => (k as KIColoredUI).ColorsUpdated(this));
        }
    #endif
    }
}
