using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code._AKFramework.AKUI.Runtime.Interfaces;
using _Source.Code._Core.View;
using SFramework.UI.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace _Source.Code._AKFramework.AKUI.Runtime
{
    [RequireComponent(typeof(Button))]
    public class ButtonView : AKView
    {
        protected IAKUIService uiService { get; set; }

        [SerializeField]
        private AKButton button;

        private Image _image;
        protected Button _button;

        protected override void PreInit()
        {
            _image = GetComponent<Image>();
            _button = GetComponent<Button>();
        }

        protected override void Init()
        {
            _button.onClick.AddListener(OnButtonClick);
        }
        
        [AKInject]
        public void _InitializeButtonInternal(IAKUIService uiController)
        {
            if(uiService != null) return;
            uiService = uiController;
            uiService.Register(button, transform as RectTransform);
        }

        private void OnButtonClick()
        {
            uiService.ButtonClickCallback(button);
            ButtonClick();
        }

        protected virtual void ButtonClick()
        {
            
        }

        protected virtual void OnDestroy()
        {
            uiService.Unregister(button, transform as RectTransform);
            _button.onClick.RemoveListener(OnButtonClick);
        }
    }
}