using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Khoai
{
    public class ColorSelectorAttribute : PropertyAttribute { }

    [ExecuteAlways]
    public class KUIColorBinder : KMonoBehaviour
    {
        public KColorPalette palette;
        public Component target;
        [ColorSelector] public int color;

        void OnValidate()
        {
            if (!target)
            {
                target = GetComponent<Graphic>();
                if (!target) target = GetComponent<TMP_Text>();
            }
            UpdateColor();
        }

        void OnEnable()
        {
#if UNITY_EDITOR
            if (palette) palette.Changed += UpdateColor;
#endif
            UpdateColor();
        }

        void OnDisable()
        {
#if UNITY_EDITOR
            if (palette) palette.Changed -= UpdateColor;
#endif
        }

        void UpdateColor()
        {
            if (!palette || !target) return;
            var c = palette.colors[color];
            if (target is Graphic g) g.color = c;
            else if (target is TMP_Text t) t.color = c;
            else Debug.LogWarning($"{target.name} has no color property.", this);
        }
    }
}