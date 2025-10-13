#if !UNITY_ADDRESSABLES_EXIST
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Source.Code._AKFramework.AKCore.Runtime;
using UnityEngine.SceneManagement;

namespace _Source.Code._AKFramework.AKScenes.Runtime
{
    public class AKScenesService : IAKScenesService
    {
        public event Action<AKScene> OnSceneLoad = s => { };
        public event Action<AKScene> OnSceneUnload = s => { };
        public event Action<AKScene> OnSceneLoaded = s => { };
        public event Action<AKScene> OnSceneUnloaded = s => { };

        private readonly List<AKScene> _loadingScenes = new List<AKScene>();
        private readonly Dictionary<AKScene, string> _availableScenes = new Dictionary<AKScene, string>();
        private readonly Dictionary<AKScene, Scene> _loadedScenes = new Dictionary<AKScene, Scene>();
        private readonly Dictionary<Scene, AKScene> _sceneToAKScene = new Dictionary<Scene, AKScene>();
        private readonly Dictionary<AKScenesGroup, AKScene[]> _scenesMap = new Dictionary<AKScenesGroup, AKScene[]>();

        [AKInject]
        private void Init(AKScenesDatabase database)
        {
            foreach (var groupContainer in database.Groups)
            {
                var akSceneGroup = new AKScenesGroup(groupContainer._Id, groupContainer._Name);
                var akScenes = new AKScene[groupContainer.Scenes.Length];
                var i = 0;
                foreach (var sceneContainer in groupContainer.Scenes)
                {
                    var akScene = new AKScene(sceneContainer._Id, sceneContainer._Name);
                    akScenes[i] = akScene;
                    _availableScenes[akScene] =
                        sceneContainer.Scene;
                    i++;
                }

                _scenesMap[akSceneGroup] = akScenes;
            }
        }

        public bool IsLoading(AKScene akScene)
        {
            return _loadingScenes.Contains(akScene);
        }

        public bool IsLoading()
        {
            return _loadingScenes.Count > 0;
        }

        public bool IsLoaded(AKScene akScene)
        {
            return _loadedScenes.ContainsKey(akScene);
        }

        public Scene GetScene(AKScene scene)
        {
            return !_loadedScenes.ContainsKey(scene) ? new Scene() : _loadedScenes[scene];
        }

        public bool GetActiveScene(out AKScene akScene)
        {
            var activeScene = SceneManager.GetActiveScene();

            if (_sceneToAKScene.ContainsKey(activeScene))
            {
                akScene = _sceneToAKScene[activeScene];
                return true;
            }

            akScene = new AKScene();
            return false;
        }

        public async void LoadScene(AKScene akScene, bool setActive, Action<Scene> loadCallback)
        {
            if (!_availableScenes.ContainsKey(akScene)) return;
            _loadingScenes.Add(akScene);
            OnSceneLoad.Invoke(akScene);
            var assetReference = _availableScenes[akScene];

            var asyncLoad = SceneManager.LoadSceneAsync(assetReference, LoadSceneMode.Additive);

            while (!asyncLoad.isDone)
            {
                await Task.Yield();
            }

            var scene = SceneManager.GetSceneByPath(assetReference);
            _loadingScenes.Remove(akScene);
            _loadedScenes[akScene] = scene;
            _sceneToAKScene[scene] = akScene;
            if (setActive)
                SceneManager.SetActiveScene(scene);
            loadCallback?.Invoke(scene);
            OnSceneLoaded.Invoke(akScene);
        }

        public async void UnloadScene(AKScene akScene, Action unloadCallback = null)
        {
            if (!_loadedScenes.ContainsKey(akScene)) return;
            _loadingScenes.Add(akScene);
            OnSceneUnload.Invoke(akScene);

            var sceneInstance = _loadedScenes[akScene];

            var asyncLoad = SceneManager.UnloadSceneAsync(sceneInstance);

            while (!asyncLoad.isDone)
            {
                await Task.Yield();
            }

            _loadingScenes.Remove(akScene);
            _loadedScenes.Remove(akScene);
            _sceneToAKScene.Remove(sceneInstance);
            unloadCallback?.Invoke();
            OnSceneUnloaded.Invoke(akScene);
        }

        public async void ReloadScene(AKScene akScene, Action unloadCallback = null,
            Action<Scene> loadCallback = null)
        {
            var isActiveScene = false;

            if (GetActiveScene(out AKScene activeScene))
            {
                if (akScene == activeScene)
                {
                    isActiveScene = true;
                }
            }

            if (!_loadedScenes.ContainsKey(akScene)) return;
            _loadingScenes.Add(akScene);
            OnSceneUnload.Invoke(akScene);

            var sceneInstance = _loadedScenes[akScene];

            var asyncLoad = SceneManager.UnloadSceneAsync(sceneInstance);

            while (!asyncLoad.isDone)
            {
                await Task.Yield();
            }

            _loadingScenes.Remove(akScene);
            _loadedScenes.Remove(akScene);
            _sceneToAKScene.Remove(sceneInstance);
            unloadCallback?.Invoke();
            OnSceneUnloaded.Invoke(akScene);

            if (!_availableScenes.ContainsKey(akScene)) return;
            _loadingScenes.Add(akScene);
            OnSceneLoad.Invoke(akScene);
            var assetReference = _availableScenes[akScene];

            asyncLoad = SceneManager.LoadSceneAsync(assetReference, LoadSceneMode.Additive);

            while (!asyncLoad.isDone)
            {
                await Task.Yield();
            }

            var scene = SceneManager.GetSceneByPath(assetReference);
            _loadingScenes.Remove(akScene);
            _loadedScenes[akScene] = scene;
            _sceneToAKScene[scene] = akScene;
            if (isActiveScene)
                SceneManager.SetActiveScene(scene);
            loadCallback?.Invoke(scene);
            OnSceneLoaded.Invoke(akScene);
        }

        public AKScene[] GetAKScenes(AKScenesGroup scenesGroup)
        {
            return _scenesMap.ContainsKey(scenesGroup) ? _scenesMap[scenesGroup] : Array.Empty<AKScene>();
        }
    }
}
#endif
