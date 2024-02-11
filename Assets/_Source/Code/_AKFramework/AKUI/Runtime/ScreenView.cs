using SFramework.UI.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Source.Code._AKFramework.AKUI.Runtime
{
    public class ScreenView : AKScreenView
    {
        protected CanvasGroup _canvasGroup;

        protected override void PreInit()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        
        protected override void OnShowScreen()
        {
            if (_canvasGroup == null) return;
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        protected override void OnScreenShown()
        {
            ScreenShownCallback();
        }

        protected override void OnCloseScreen()
        {
            if (_canvasGroup == null) return;
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        protected override void OnScreenClosed()
        {
            ScreenClosedCallback();
        }

        #if UNITY_EDITOR

        [HorizontalGroup("Button")]
        [Button(Name = "Show")]
        private void ShowScreenButton()
        {
            ShowScreen();
        }

        [HorizontalGroup("Button")]
        [Button(Name = "Close")]
        private void CloseScreenButton()
        {
            CloseScreen();
        }
        
        #endif
    }
}