using System;
using System.Collections.Generic;
using _Source.Code._AKFramework.AKUI.Runtime.Interfaces;
using SFramework.UI.Runtime;
using UnityEngine;
using Zenject;

namespace _Source.Code._AKFramework.AKUI.Runtime
{
    public sealed class AKUIService : IAKUIService
    {
        private AKUIDatabase _database;

        public bool IsLoaded(AKScreen screen)
        {
            if (screen == null || screen.IsNone) return _isLoaded;
            if (!_isLoaded) return false;
            return _screenRootGameObjects.ContainsKey(screen) && _screenRootGameObjects[screen] != null;
        }

        public event Action<AKScreen> OnShowScreen = screen => { };
        public event Action<AKScreen> OnCloseScreen = screen => { };
        public event Action<AKScreen> OnScreenShown = screen => { };
        public event Action<AKScreen> OnScreenClosed = screen => { };
        public event Action<AKButton> OnButtonClick = button => { };
        public event Action<AKToggle, bool> OnToggleClick = (toggle, value) => { };

        private readonly Dictionary<AKScreen, AKScreenState> _screenStates = new();
        private readonly Dictionary<AKScreen, GameObject> _screenRootGameObjects = new();
        private readonly Dictionary<AKButton, List<RectTransform>> _buttonsMap = new();

        private bool _isLoaded;

        [Inject]
        public void Init(AKUIDatabase database)
        {
            _database = database;
        }

        public void ShowScreen(AKScreen screen)
        {
            if (screen.IsNone) return;
            if(!_screenStates.ContainsKey(screen)) return;
            _screenStates[screen] = AKScreenState.Showing;
            OnShowScreen.Invoke(screen);
        }

        public void CloseScreen(AKScreen screen)
        {
            if (screen.IsNone) return;
            if(!_screenStates.ContainsKey(screen)) return;
            _screenStates[screen] = AKScreenState.Closing;
            OnCloseScreen.Invoke(screen);
        }

        public AKScreenState GetScreenState(AKScreen screen)
        {
            return !_screenStates.ContainsKey(screen) ? AKScreenState.Closed : _screenStates[screen];
        }

        public T GetButtonData<T>(AKButton button) where T : AKButtonData
        {
            foreach (var screenGroupContainer in _database.ScreenGroupsContainers)
            {
                foreach (var screenContainer in screenGroupContainer.ScreenContainers)
                {
                    foreach (var buttonContainer in screenContainer.ButtonContainers)
                    {
                        if (button._Id == buttonContainer._Id)
                        {
                            return buttonContainer.ButtonData as T;
                        }
                    }
                }
            }

            return ScriptableObject.CreateInstance<T>();
        }

        public GameObject GetScreenRootGO(AKScreen screen)
        {
            return !_screenRootGameObjects.ContainsKey(screen) ? null : _screenRootGameObjects[screen];
        }

        public RectTransform GetButton(AKButton button, int id = 0)
        {
            return _buttonsMap.ContainsKey(button) ? _buttonsMap[button][id] : null;
        }

        public void Register(AKScreen screen, GameObject root)
        {
            if (!_isLoaded)
            {
                _isLoaded = true;
            }

            _screenStates[screen] = AKScreenState.Closed;
            _screenRootGameObjects[screen] = root;
        }

        public void Unregister(AKScreen screen)
        {
            if (_screenStates.ContainsKey(screen))
                _screenStates.Remove(screen);

            if (_screenRootGameObjects.ContainsKey(screen))
                _screenRootGameObjects.Remove(screen);
        }

        public void Register(AKButton sfButton, RectTransform button)
        {
            if (_buttonsMap.ContainsKey(sfButton))
            {
                _buttonsMap[sfButton].Add(button);
            }
            else
            {
                _buttonsMap[sfButton] = new List<RectTransform>() { button };
            }
        }

        public void Unregister(AKButton akButton, RectTransform button)
        {
            if (!_buttonsMap.ContainsKey(akButton)) return;
            
            if (_buttonsMap[akButton].Count > 1)
            {
                _buttonsMap[akButton].Remove(button);
            }
            else
            {
                _buttonsMap.Remove(akButton);
            }
        }

        public void ScreenShownCallback(AKScreen akScreen)
        {
            if (akScreen.IsNone) return;
            _screenStates[akScreen] = AKScreenState.Shown;
            OnScreenShown.Invoke(akScreen);
        }

        public void ScreenClosedCallback(AKScreen akScreen)
        {
            if (akScreen.IsNone) return;
            _screenStates[akScreen] = AKScreenState.Closed;
            OnScreenClosed.Invoke(akScreen);
        }

        public void ButtonClickCallback(AKButton akButton)
        {
            if (akButton.IsNone) return;
            OnButtonClick.Invoke(akButton);
        }

        public void ToggleClickCallback(AKToggle akToggle, bool value)
        {
            if (akToggle.IsNone) return;
            OnToggleClick.Invoke(akToggle, value);
        }
    }
}