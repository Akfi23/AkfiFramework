#if UNITY_ADDRESSABLES_EXIST

using System;
using UnityEngine.ResourceManagement.ResourceProviders;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace _Source.Code._AKFramework.AKScenes.Runtime
{
    public interface IAKScenesService : IAKService
    {
        event Action<AKScene> OnSceneLoad;
        event Action<AKScene> OnSceneUnload;
        event Action<AKScene> OnSceneLoaded;
        event Action<AKScene> OnSceneUnloaded;
        bool IsLoading(AKScene akScene);
        bool IsLoading();
        bool IsLoaded(AKScene akScene);
        AKScene[] GetAKScenes(AKScenesGroup scenesGroup);
        bool GetActiveScene(out AKScene akScene);
        SceneInstance GetScene(AKScene akScene);
        bool GetActiveScene(out SceneInstance sceneInstance);
        Task<SceneInstance> LoadScene(AKScene akScene, bool setActive, Action<SceneInstance> loadCallback = null);
        Task UnloadScene(AKScene akScene, Action unloadCallback = null);
        Task<SceneInstance> ReloadScene(AKScene akScene, Action unloadCallback = null, Action<SceneInstance> loadCallback = null);
    }
}
#endif
