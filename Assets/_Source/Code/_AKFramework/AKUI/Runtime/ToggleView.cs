using _Source.Code._AKFramework.AKUI.Runtime.Interfaces;
using _Source.Code._Core.View;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Source.Code._AKFramework.AKUI.Runtime
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleView : AKView
    {
        [Inject]
        protected IAKUIService uiService { get; set; }

        [Inject]
        protected IAKUIService uiCallbacks { get; set; }

        [SerializeField]
        private AKToggle toggle;
        
        protected Toggle _toggle;
        
        protected override void PreInit()
        {
            _toggle = GetComponent<Toggle>();
        }
        
        protected override void Init()
        {
            _toggle.onValueChanged.AddListener(OnToggleClick);
        }
        
        private void OnToggleClick(bool value)
        {
            uiCallbacks.ToggleClickCallback(toggle, value);
            ToggleClick(value);
        }

        protected virtual void ToggleClick(bool value)
        {
            
        }

        protected virtual void OnDestroy()
        {
            _toggle.onValueChanged.RemoveListener(OnToggleClick);
        }
    }
}
