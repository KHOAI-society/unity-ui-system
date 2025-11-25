#if UNITY_EDITOR

using System.CodeDom;
using System.Collections.Generic;
using Khoai.UI;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Khoai.Editors
{
    public static class EditorUtilities
    {
        private const string kUILayerName = "UI";

        private const string kStandardSpritePath = "UI/Skin/UISprite.psd";
        private const string kBackgroundSpritePath = "UI/Skin/Background.psd";
        private const string kInputFieldBackgroundPath = "UI/Skin/InputFieldBackground.psd";
        private const string kKnobPath = "UI/Skin/Knob.psd";
        private const string kCheckmarkPath = "UI/Skin/Checkmark.psd";
        private const string kDropdownArrowPath = "UI/Skin/DropdownArrow.psd";
        private const string kMaskPath = "UI/Skin/UIMask.psd";

        // Actual action
        [MenuItem("GameObject/Khoai/Copy Path", false, 0)]
        private static void LogSelectedPath()
        {
            var t = Selection.activeGameObject.transform;
            var names = new List<string>();
            while (t != null) { names.Add(t.name); t = t.parent; }
            names.Reverse();
            GUIUtility.systemCopyBuffer = string.Join("/", names);
        }

        // Validation so it shows only when a GameObject is selected
        [MenuItem("GameObject/Khoai/Copy Path", true)]
        private static bool LogSelectedPathValidate() => Selection.activeGameObject != null;

        [MenuItem("GameObject/Khoai/Add Themed Components")]
        public static void AddThemedComponents()
        {
            if (Selection.activeGameObject == null) return;

            Undo.IncrementCurrentGroup();
            var undoGroup = Undo.GetCurrentGroup();
            AddThemedComponents(Selection.activeGameObject.transform);
            Undo.CollapseUndoOperations(undoGroup);
        }

        private static void AddThemedComponents(Transform tf)
        {
            foreach (Transform child in tf)
            {
                AddThemedComponents(child);
            }

            if (tf.TryGetComponent<KThemedItem>(out var _)) return;
            if (tf.TryGetComponent<Text>(out var _))
            {
                Undo.AddComponent<KThemedText>(tf.gameObject);
                return;
            }
            else if (tf.TryGetComponent<TextMeshProUGUI>(out var _))
            {
                Undo.AddComponent<KThemedTextMeshProUGUI>(tf.gameObject);
                return;
            }
            else if (tf.TryGetComponent<Image>(out var _))
            {
                Undo.AddComponent<KThemedImage>(tf.gameObject);
                return;
            }
        }

        [MenuItem("GameObject/Khoai/Create Switch")]
        public static void AddSwitch()
        {
            Vector2 size = new(150f, 60f);
            Vector2 switchSize = new(60f, 30f);
            Vector2 handleSize = new(switchSize.x / 2, switchSize.y);
            Vector2 titleSize = new(size.x, size.y - switchSize.y);
            Vector2 sideSize = new((size.x - switchSize.x) / 2, switchSize.y);

            Transform parent = null; ;
            if (Selection.activeGameObject)
                parent = Selection.activeGameObject.transform;

            Undo.IncrementCurrentGroup();
            var undoGroup = Undo.GetCurrentGroup();

            GameObject top = new("KSwitch");
            var topTransform = top.AddComponent<RectTransform>();
            topTransform.SetParent(parent);
            CenterRectTransform(topTransform, size);

            GameObject title = new("Title");
            var titleTransform = title.AddComponent<RectTransform>();
            titleTransform.SetParent(topTransform);
            titleTransform.pivot = new Vector2(0.5f, 0.5f);
            titleTransform.localScale = Vector3.one;
            titleTransform.anchorMin = new Vector2(0.5f, 0.75f);
            titleTransform.anchorMax = new Vector2(0.5f, 0.75f);
            SetSize(titleTransform, titleSize);

            var titleText = title.AddComponent<TextMeshProUGUI>();
            titleText.text = "Title";
            titleText.fontSize = 14;
            titleText.alignment = TextAlignmentOptions.Center;

            GameObject switchGo = new("Switch");
            var switchTransform = switchGo.AddComponent<RectTransform>();
            switchTransform.SetParent(topTransform);
            switchTransform.pivot = new Vector2(0.5f, 0.5f);
            switchTransform.localScale = Vector3.one;
            switchTransform.anchorMin = new Vector2(0.5f, 0.25f);
            switchTransform.anchorMax = new Vector2(0.5f, 0.25f);
            SetSize(switchTransform, switchSize);

            var image = switchGo.AddComponent<Image>();
            image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePath);
            image.type = Image.Type.Sliced;

            GameObject filledGo = new("Filled");
            var filledTransform = filledGo.AddComponent<RectTransform>();
            filledTransform.SetParent(switchTransform);
            filledTransform.pivot = new Vector2(0.5f, 0.5f);
            filledTransform.localScale = Vector3.one;
            filledTransform.anchorMin = new Vector2(0f, 0f);
            filledTransform.anchorMax = new Vector2(1f, 1f);
            filledTransform.offsetMin = Vector2.zero;
            filledTransform.offsetMax = Vector2.zero;
            var filledImage = filledGo.AddComponent<Image>();
            filledImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePath);
            filledImage.color = Color.red;
            filledImage.type = Image.Type.Sliced;

            GameObject handleGo = new("Handle");
            var handleTransform = handleGo.AddComponent<RectTransform>();
            handleTransform.SetParent(switchTransform);
            handleTransform.pivot = new Vector2(0f, 0.5f);
            handleTransform.localScale = Vector3.one;
            handleTransform.anchorMin = new Vector2(0f, 0.5f);
            handleTransform.anchorMax = new Vector2(0f, 0.5f);
            handleTransform.offsetMin = new(0f, -handleSize.y / 2);
            handleTransform.offsetMax = new(handleSize.x, handleSize.y / 2);
            var handleImage = handleGo.AddComponent<Image>();
            handleImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePath);
            handleImage.type = Image.Type.Sliced;

            GameObject offGo = new("Off");
            var offTransform = offGo.AddComponent<RectTransform>();
            offTransform.SetParent(topTransform);
            offTransform.pivot = new Vector2(0f, 0.5f);
            offTransform.localScale = Vector3.one;
            offTransform.anchorMin = new Vector2(0f, 0.25f);
            offTransform.anchorMax = new Vector2(0f, 0.25f);
            offTransform.offsetMin = new(0f, -sideSize.y / 2);
            offTransform.offsetMax = new(sideSize.x, sideSize.y / 2);
            var offText = offGo.AddComponent<TextMeshProUGUI>();
            offText.text = "OFF";
            offText.fontSize = 14;
            offText.alignment = TextAlignmentOptions.Left;

            GameObject onGo = new("On");
            var onTransform = onGo.AddComponent<RectTransform>();
            onTransform.SetParent(topTransform);
            onTransform.pivot = new Vector2(1f, 0.5f);
            onTransform.localScale = Vector3.one;
            onTransform.anchorMin = new Vector2(1f, 0.25f);
            onTransform.anchorMax = new Vector2(1f, 0.25f);
            onTransform.offsetMin = new(-sideSize.x, -sideSize.y / 2);
            onTransform.offsetMax = new(0f, sideSize.y / 2);
            var onText = onGo.AddComponent<TextMeshProUGUI>();
            onText.text = "ON";
            onText.fontSize = 14;
            onText.alignment = TextAlignmentOptions.Right;

            switchGo.AddComponent<KSwtich>();
            SetLayerRecursively(topTransform, LayerMask.NameToLayer(kUILayerName));

            AddThemedComponents(top.transform);

            Undo.CollapseUndoOperations(undoGroup);
        }

        private static void CenterRectTransform(RectTransform rectTransform, Vector2 size)
        {
            rectTransform.anchorMax = Vector2.one / 2;
            rectTransform.anchorMin = Vector2.one / 2;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.localScale = Vector2.one;
            rectTransform.position = Vector2.zero;
            SetSize(rectTransform, size);
        }

        private static void SetSize(RectTransform rectTransform, Vector2 size)
        {
            rectTransform.offsetMin = size / 2 * -1;
            rectTransform.offsetMax = size / 2;
        }

        private static void SetLayerRecursively(Transform transform, int layer)
        {
            transform.gameObject.layer = layer;
            foreach(Transform child in transform)
                SetLayerRecursively(child, layer);
        }
    }
}

#endif