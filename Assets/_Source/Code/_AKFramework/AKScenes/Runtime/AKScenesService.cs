#if !UNITY_ADDRESSABLES_EXIST
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Zenject;

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
        private readonly Dictionary<Scene, AKScene> _sceneToSFScene = new Dictionary<Scene, AKScene>();
        private readonly Dictionary<AKScenesGroup, AKScene[]> _scenesMap = new Dictionary<AKScenesGroup, AKScene[]>();

        [Inject]
        private void Init(AKScenesDatabase database)
        {
            foreach (var groupContainer in database.Groups)
            {
                var sfSceneGroup = new AKScenesGroup(groupContainer._Id, groupContainer._Name);
                var sfScenes = new AKScene[groupContainer.Scenes.Length];
                var i = 0;
                foreach (var sceneContainer in groupContainer.Scenes)
                {
                    var sfScene = new AKScene(sceneContainer._Id, sceneContainer._Name);
                    sfScenes[i] = sfScene;
                    _availableScenes[sfScene] =
                        sceneContainer.Scene;
                    i++;
                }

                _scenesMap[sfSceneGroup] = sfScenes;
            }
        }

        public bool IsLoading(AKScene sfScene)
        {
            return _loadingScenes.Contains(sfScene);
        }

        public bool IsLoading()
        {
            return _loadingScenes.Count > 0;
        }

        public bool IsLoaded(AKScene sfScene)
        {
            return _loadedScenes.ContainsKey(sfScene);
        }

        public Scene GetScene(AKScene scene)
        {
            return !_loadedScenes.ContainsKey(scene) ? new Scene() : _loadedScenes[scene];
        }

        public bool GetActiveScene(out AKScene sfScene)
        {
            var activeScene = SceneManager.GetActiveScene();

            if (_sceneToSFScene.ContainsKey(activeScene))
            {
                sfScene = _sceneToSFScene[activeScene];
                return true;
            }

            sfScene = new AKScene();
            return false;
        }

        public async void LoadScene(AKScene sfScene, bool setActive, Action<Scene> loadCallback)
        {
            if (!_availableScenes.ContainsKey(sfScene)) return;
            _loadingScenes.Add(sfScene);
            OnSceneLoad.Invoke(sfScene);
            var assetReference = _availableScenes[sfScene];

            var asyncLoad = SceneManager.LoadSceneAsync(assetReference, LoadSceneMode.Additive);

            while (!asyncLoad.isDone)
            {
                await Task.Yield();
            }

            var scene = SceneManager.GetSceneByPath(assetReference);
            _loadingScenes.Remove(sfScene);
            _loadedScenes[sfScene] = scene;
            _sceneToSFScene[scene] = sfScene;
            if (setActive)
                SceneManager.SetActiveScene(scene);
            loadCallback?.Invoke(scene);
            OnSceneLoaded.Invoke(sfScene);
        }

        public async void UnloadScene(AKScene sfScene, Action unloadCallback = null)
        {
            if (!_loadedScenes.ContainsKey(sfScene)) return;
            _loadingScenes.Add(sfScene);
            OnSceneUnload.Invoke(sfScene);

            var sceneInstance = _loadedScenes[sfScene];

            var asyncLoad = SceneManager.UnloadSceneAsync(sceneInstance);

            while (!asyncLoad.isDone)
            {
                await Task.Yield();
            }

            _loadingScenes.Remove(sfScene);
            _loadedScenes.Remove(sfScene);
            _sceneToSFScene.Remove(sceneInstance);
            unloadCallback?.Invoke();
            OnSceneUnloaded.Invoke(sfScene);
        }

        public async void ReloadScene(AKScene sfScene, Action unloadCallback = null,
            Action<Scene> loadCallback = null)
        {
            var isActiveScene = false;

            if (GetActiveScene(out AKScene activeScene))
            {
                if (sfScene == activeScene)
                {
                    isActiveScene = true;
                }
            }

            if (!_loadedScenes.ContainsKey(sfScene)) return;
            _loadingScenes.Add(sfScene);
            OnSceneUnload.Invoke(sfScene);

            var sceneInstance = _loadedScenes[sfScene];

            var asyncLoad = SceneManager.UnloadSceneAsync(sceneInstance);

            while (!asyncLoad.isDone)
            {
                await Task.Yield();
            }

            _loadingScenes.Remove(sfScene);
            _loadedScenes.Remove(sfScene);
            _sceneToSFScene.Remove(sceneInstance);
            unloadCallback?.Invoke();
            OnSceneUnloaded.Invoke(sfScene);

            if (!_availableScenes.ContainsKey(sfScene)) return;
            _loadingScenes.Add(sfScene);
            OnSceneLoad.Invoke(sfScene);
            var assetReference = _availableScenes[sfScene];

            asyncLoad = SceneManager.LoadSceneAsync(assetReference, LoadSceneMode.Additive);

            while (!asyncLoad.isDone)
            {
                await Task.Yield();
            }

            var scene = SceneManager.GetSceneByPath(assetReference);
            _loadingScenes.Remove(sfScene);
            _loadedScenes[sfScene] = scene;
            _sceneToSFScene[scene] = sfScene;
            if (isActiveScene)
                SceneManager.SetActiveScene(scene);
            loadCallback?.Invoke(scene);
            OnSceneLoaded.Invoke(sfScene);
        }

        public AKScene[] GetSFScenes(AKScenesGroup scenesGroup)
        {
            return _scenesMap.ContainsKey(scenesGroup) ? _scenesMap[scenesGroup] : Array.Empty<AKScene>();
        }
    }
}
#endif