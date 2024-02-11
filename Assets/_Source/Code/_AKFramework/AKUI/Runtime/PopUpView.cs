using SFramework.UI.Runtime;
using UnityEngine;

namespace _Source.Code._AKFramework.AKUI.Runtime
{
    public class PopUpView : ScreenView
    {
        [SerializeField]
        private bool hideByDefault = true;

        protected override void Init()
        {
            _canvasGroup.alpha = hideByDefault ? 0f : 1f;
            _canvasGroup.interactable = !hideByDefault;
            _canvasGroup.blocksRaycasts = !hideByDefault;
        }
    }
}