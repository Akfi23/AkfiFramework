using System.Collections.Generic;
using System.Threading.Tasks;
using _Source.Code._AKFramework.AKUI.Runtime.Interfaces;
using SFramework.UI.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Source.Code._AKFramework.AKUI.Runtime
{
    public class ScreenAnimationView : AKScreenView
    {
        [SerializeField]
        private bool hideByDefault = true;
        
        [SerializeReference] 
        private IAKScreenAnimation[] showAnimations;
        [SerializeReference] 
        private IAKScreenAnimation[] closeAnimations;

        protected CanvasGroup _canvasGroup;
        private readonly List<Task> _tasks = new();

        protected override void PreInit()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        
        protected override void Init()
        {
            _canvasGroup.alpha = hideByDefault ? 0f : 1f;
            _canvasGroup.interactable = !hideByDefault;
            _canvasGroup.blocksRaycasts = !hideByDefault;
        }
        
        protected override async void OnShowScreen()
        {
            if (_canvasGroup == null) return;
            
            _canvasGroup.alpha = 1f;

            if (!_canvasGroup.interactable)
            {
                await OnShowAnimation();
            }

            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        protected override void OnScreenShown()
        {
            ScreenShownCallback();
        }

        protected override async void OnCloseScreen()
        {
            if (_canvasGroup == null) return;
            
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            if (_canvasGroup.alpha != 0f)
            {
                await OnCloseAnimation();
            }

            _canvasGroup.alpha = 0f;
        }

        protected override void OnScreenClosed()
        {
            ScreenClosedCallback();
        }

        protected virtual async Task OnShowAnimation()
        {
            foreach (var anim in closeAnimations)
            {
                anim.Reset();
            }
            
            _tasks.Clear();
            
            for (int i = 0; i < showAnimations.Length; i++)
            {
                _tasks.Add(showAnimations[i].DoAction(_canvasGroup));
            }

            await Task.WhenAll(_tasks);
        }
        
        protected virtual async Task OnCloseAnimation()
        {
            foreach (var anim in showAnimations)
            {
                anim.Reset();
            }
            
            _tasks.Clear();
            
            for (int i = 0; i < closeAnimations.Length; i++)
            {
                _tasks.Add(closeAnimations[i].DoAction(_canvasGroup));
            }

            await Task.WhenAll(_tasks);
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
