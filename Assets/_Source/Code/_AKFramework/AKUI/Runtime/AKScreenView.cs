using _Source.Code._AKFramework.AKUI.Runtime.Interfaces;
using _Source.Code._Core.View;
using SFramework.UI.Runtime;
using UnityEngine;
using Zenject;

namespace _Source.Code._AKFramework.AKUI.Runtime
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class AKScreenView : AKView
    {
        public Canvas Canvas => _canvas;
        
        public AKScreen Screen => _screen;

        protected AKScreenState State => _uiServiceInternal.GetScreenState(_screen);

        [SerializeField]
        private AKScreen _screen;

        private Canvas _canvas;

        private IAKUIService _uiServiceInternal;

        protected override void Awake()
        {
            _canvas = GetComponent<Canvas>();
            base.Awake();
        }

        [Inject]
        public void _InitializeScreenInternal(IAKUIService uiController)
        {
            if(_uiServiceInternal != null) return;
            _uiServiceInternal = uiController;
            _uiServiceInternal.Register(_screen, gameObject);
            _uiServiceInternal.OnShowScreen += _onShowScreen;
            _uiServiceInternal.OnCloseScreen += _onCloseScreen;
            _uiServiceInternal.OnScreenShown += _onScreenShown;
            _uiServiceInternal.OnScreenClosed += _onScreenClosed;
        }


        protected abstract void OnShowScreen();
        protected abstract void OnScreenShown();
        protected abstract void OnCloseScreen();
        protected abstract void OnScreenClosed();

        public void ShowScreen()
        {
            _uiServiceInternal.ShowScreen(Screen);
        }

        public void CloseScreen()
        {
            _uiServiceInternal.CloseScreen(Screen);
        }

        public void ScreenShownCallback()
        {
            _uiServiceInternal.ScreenShownCallback(_screen);
        }

        public void ScreenClosedCallback()
        {
            _uiServiceInternal.ScreenClosedCallback(_screen);
        }

        private void _onShowScreen(AKScreen screen)
        {
            if (screen != _screen) return;
            OnShowScreen();
        }

        private void _onScreenShown(AKScreen screen)
        {
            if (screen != _screen) return;
            OnScreenShown();
        }

        private void _onCloseScreen(AKScreen screen)
        {
            if (screen != _screen) return;
            OnCloseScreen();
        }

        private void _onScreenClosed(AKScreen screen)
        {
            if (screen != _screen) return;
            OnScreenClosed();
        }

        protected virtual void OnDestroy()
        {
            _uiServiceInternal.Unregister(_screen);
            _uiServiceInternal.OnShowScreen -= _onShowScreen;
            _uiServiceInternal.OnCloseScreen -= _onCloseScreen;
            _uiServiceInternal.OnScreenShown -= _onScreenShown;
            _uiServiceInternal.OnScreenClosed -= _onScreenClosed;
        }
    }
}