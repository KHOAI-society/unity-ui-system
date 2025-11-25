using System;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Khoai.UI
{
    public class KSwtich : KSelectable, IPointerClickHandler
    {
        [Serializable] public class SwitchClickedEvent : UnityEvent<bool> { }
        [SerializeField] private SwitchClickedEvent m_OnSwitch = new();
        [SerializeField] private bool isOn;

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
            m_OnSwitch.Invoke(isOn);
            SetSwitchUI();
        }

        protected override void Start()
        {
            base.Start();
            SetSwitchUI();
        }

        private void SetSwitchUI()
        {
            Debug.Log($"switch is {isOn}");
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            Debug.Log($"OnValidate is {isOn}");   
        }
#endif
    }
}