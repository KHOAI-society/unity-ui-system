using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Khoai.UI
{
    public class KSwtich : KSelectable, IPointerClickHandler
    {
        [Serializable] public class SwitchClickedEvent : UnityEvent<bool> { }
        [SerializeField] private SwitchClickedEvent m_OnSwitch = new();
        [SerializeField] private bool isOn;
        [SerializeField] private TextMeshProUGUI offText;
        [SerializeField] private TextMeshProUGUI onText;
        [SerializeField] private Image background;
        [SerializeField] private RectTransform handleTransform;

        public SwitchClickedEvent OnSwitch
        {
            get { return m_OnSwitch; }
            set { m_OnSwitch = value; }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!IsActive() || !IsInteractable())
                return;

            UISystemProfilerApi.AddMarker("KSwitch.OnPointerClick", this);
            isOn = !isOn;
            m_OnSwitch?.Invoke(isOn);
            SetSwitchUI();
        }

        protected override void Start()
        {
            base.Start();
            SetSwitchUI();
        }

        protected virtual void SetSwitchUI()
        {
            if(onText == null) return;
            if(offText == null) return;
            if(background == null) return;
            if(handleTransform == null) return;
            onText.color = isOn ? Color.green : Color.white;
            offText.color = isOn ? Color.white : Color.red;
            background.color = isOn? Color.green : Color.red;
            float anchor = isOn? 1f : 0f;
            handleTransform.pivot = new Vector2(anchor, 0.5f);
            handleTransform.localScale = Vector3.one;
            handleTransform.anchorMin = new Vector2(anchor, 0.5f);
            handleTransform.anchorMax = new Vector2(anchor, 0.5f);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            SetSwitchUI(); 
        }
#endif
    }
}