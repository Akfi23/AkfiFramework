using System;
using SFramework.UI.Runtime;
using UnityEngine;

namespace _Source.Code._AKFramework.AKUI.Runtime.Interfaces
{
    public interface IAKUIService
    {

        bool IsLoaded(AKScreen screen);

        event Action<AKScreen> OnShowScreen;
        event Action<AKScreen> OnCloseScreen;
        event Action<AKScreen> OnScreenShown;
        event Action<AKScreen> OnScreenClosed;

        event Action<AKButton> OnButtonClick;
        event Action<AKToggle, bool> OnToggleClick;

        /// <summary>
        /// Invokes OnShowScreen event
        /// </summary>
        /// <param name="screen">AKScreen</param>
        void ShowScreen(AKScreen screen);

        /// <summary>
        /// Invokes OnCloseScreen event
        /// </summary>
        /// <param name="screen"></param>
        void CloseScreen(AKScreen screen);
        AKScreenState GetScreenState(AKScreen screen);
        T GetButtonData<T>(AKButton button) where T : AKButtonData;

        GameObject GetScreenRootGO(AKScreen screen);
        RectTransform GetButton(AKButton button, int id = 0);
        
        void Register(AKScreen screen, GameObject root);
        void Register(AKButton akButton, RectTransform button);
        void Unregister(AKScreen screen);
        void Unregister(AKButton ak, RectTransform button);
        void ScreenShownCallback(AKScreen akScreen);
        void ScreenClosedCallback(AKScreen akScreen);

        void ButtonClickCallback(AKButton akButton);

        void ToggleClickCallback(AKToggle akToggle, bool value);
    }
}