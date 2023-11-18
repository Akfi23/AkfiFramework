#if !UNITY_ADDRESSABLES_EXIST

using System;
using UnityEngine.SceneManagement;

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
        AKScene[] GetSFScenes(AKScenesGroup scenesGroup);
        bool GetActiveScene(out AKScene akScene);
        Scene GetScene(AKScene akScene);
        void LoadScene(AKScene akScene, bool setActive, Action<Scene> loadCallback = null);
        void UnloadScene(AKScene akScene, Action unloadCallback = null);
        void ReloadScene(AKScene akScene, Action unloadCallback = null, Action<Scene> loadCallback = null);
    }
}
#endif