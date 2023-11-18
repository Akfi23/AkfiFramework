#if UNITY_ADDRESSABLES_EXIST
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Source.Code._AKFramework.AKScenes.Runtime;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Zenject;

// ReSharper disable once CheckNamespace
namespace SFramework.Scenes.Runtime
{
    public sealed class SFScenesService : IAKScenesService
    {
        public event Action<AKScene> OnSceneLoad = s => { };
        public event Action<AKScene> OnSceneUnload = s => { };
        public event Action<AKScene> OnSceneLoaded = s => { };
        public event Action<AKScene> OnSceneUnloaded = s => { };

        private readonly Dictionary<AKScene, SceneInstance> _loadedScenes = new Dictionary<AKScene, SceneInstance>();
        private readonly List<AKScene> _loadingScenes = new List<AKScene>();

        private readonly Dictionary<AKScene, AssetReference> _availableScenes =
            new Dictionary<AKScene, AssetReference>();

        private readonly Dictionary<Scene, SceneInstance>
            _sceneToSceneInstance = new Dictionary<Scene, SceneInstance>();

        private readonly Dictionary<SceneInstance, Scene>
            _sceneInstanceToScene = new Dictionary<SceneInstance, Scene>();

        private readonly Dictionary<SceneInstance, AKScene>
            _sceneInstanceToSFScene = new Dictionary<SceneInstance, AKScene>();

        private readonly Dictionary<AKScenesGroup, AKScene[]> _scenesMap = new Dictionary<AKScenesGroup, AKScene[]>();


        [Inject]
        public void Init(AKScenesDatabase database)
        {
            foreach (var groupContainer in database.Groups)
            {
                var akScenes = new AKScene[groupContainer.Scenes.Length];
                var i = 0;
                foreach (var sceneContainer in groupContainer.Scenes)
                {
                    akScenes[i] = new AKScene(sceneContainer._Id, sceneContainer._Name);
                    _availableScenes[new AKScene(sceneContainer._Id, sceneContainer._Name)] =
                        sceneContainer.Scene;
                    i++;
                }

                _scenesMap[new AKScenesGroup(groupContainer._Id, groupContainer._Name)] = akScenes;
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

        public SceneInstance GetScene(AKScene scene)
        {
            return !_loadedScenes.ContainsKey(scene) ? new SceneInstance() : _loadedScenes[scene];
        }

        public bool GetActiveScene(out SceneInstance sceneInstance)
        {
            var activeScene = SceneManager.GetActiveScene();

            if (_sceneToSceneInstance.ContainsKey(activeScene))
            {
                sceneInstance = _sceneToSceneInstance[activeScene];
                return true;
            }

            sceneInstance = new SceneInstance();
            return false;
        }

        public bool GetActiveScene(out AKScene akScene)
        {
            var activeScene = SceneManager.GetActiveScene();

            if (_sceneToSceneInstance.ContainsKey(activeScene))
            {
                var sceneInstance = _sceneToSceneInstance[activeScene];
                akScene = _sceneInstanceToSFScene[sceneInstance];
                return true;
            }

            akScene = new AKScene();
            return false;
        }

        public async Task<SceneInstance> LoadScene(AKScene akScene, bool setActive,
            Action<SceneInstance> onDone = null)
        {
            if (!_availableScenes.ContainsKey(akScene)) return new SceneInstance();
            _loadingScenes.Add(akScene);
            OnSceneLoad.Invoke(akScene);
            var assetReference = _availableScenes[akScene];
            var asyncOperationHandle = Addressables.LoadSceneAsync(assetReference.RuntimeKey, LoadSceneMode.Additive);
            await asyncOperationHandle.Task;
            var sceneInstance = asyncOperationHandle.Result;
            var scene = sceneInstance.Scene;
            _loadingScenes.Remove(akScene);
            _loadedScenes[akScene] = sceneInstance;
            _sceneInstanceToScene[sceneInstance] = scene;
            _sceneInstanceToSFScene[sceneInstance] = akScene;
            _sceneToSceneInstance[scene] = sceneInstance;
            if (setActive)
                SceneManager.SetActiveScene(scene);
            onDone?.Invoke(sceneInstance);
            OnSceneLoaded.Invoke(akScene);
            return sceneInstance;
        }

        public async Task UnloadScene(AKScene akScene, Action onDone = null)
        {
            if (!_loadedScenes.ContainsKey(akScene)) return;
            _loadingScenes.Add(akScene);
            OnSceneUnload.Invoke(akScene);
            var sceneInstance = _loadedScenes[akScene];
            var scene = _sceneInstanceToScene[sceneInstance];
            var asyncOperationHandle = Addressables.UnloadSceneAsync(sceneInstance);
            await asyncOperationHandle.Task;
            _loadingScenes.Remove(akScene);
            _loadedScenes.Remove(akScene);
            _sceneInstanceToScene.Remove(sceneInstance);
            _sceneInstanceToSFScene.Remove(sceneInstance);
            _sceneToSceneInstance.Remove(scene);
            onDone?.Invoke();
            OnSceneUnloaded.Invoke(akScene);
        }

        public async Task<SceneInstance> ReloadScene(AKScene akScene, Action onUnloaded = null,
            Action<SceneInstance> onLoaded = null)
        {
            var isActiveScene = false;

            if (GetActiveScene(out AKScene activeScene))
            {
                if (akScene == activeScene)
                {
                    isActiveScene = true;
                }
            }

            if (!_loadedScenes.ContainsKey(akScene)) return new SceneInstance();
            _loadingScenes.Add(akScene);
            OnSceneUnload.Invoke(akScene);
            var sceneInstance = _loadedScenes[akScene];
            var scene = _sceneInstanceToScene[sceneInstance];
            var asyncOperationHandle = Addressables.UnloadSceneAsync(sceneInstance);
            await asyncOperationHandle.Task;
            _loadedScenes.Remove(akScene);
            _sceneInstanceToSFScene.Remove(sceneInstance);
            _sceneInstanceToScene.Remove(sceneInstance);
            _sceneToSceneInstance.Remove(scene);
            onUnloaded?.Invoke();
            OnSceneUnloaded.Invoke(akScene);

            if (!_availableScenes.ContainsKey(akScene)) return new SceneInstance();
            OnSceneLoad.Invoke(akScene);
            var assetReference = _availableScenes[akScene];
            asyncOperationHandle = Addressables.LoadSceneAsync(assetReference.RuntimeKey, LoadSceneMode.Additive);
            await asyncOperationHandle.Task;
            sceneInstance = asyncOperationHandle.Result;
            scene = sceneInstance.Scene;
            _loadingScenes.Remove(akScene);
            _loadedScenes[akScene] = sceneInstance;
            _sceneInstanceToScene[sceneInstance] = scene;
            _sceneInstanceToSFScene[sceneInstance] = akScene;
            _sceneToSceneInstance[scene] = sceneInstance;

            if (isActiveScene)
            {
                SceneManager.SetActiveScene(scene);
            }

            onLoaded?.Invoke(sceneInstance);
            OnSceneLoaded.Invoke(akScene);
            return sceneInstance;
        }

        public AKScene[] GetAKScenes(AKScenesGroup scenesGroup)
        {
            return _scenesMap.ContainsKey(scenesGroup) ? _scenesMap[scenesGroup] : Array.Empty<AKScene>();
        }
    }
}
#endif