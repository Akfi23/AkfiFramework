using System;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace _Client_.Scripts._Root
{
    public static class PlayerLoopCleaner
    {
        private static readonly Type[] TypesToRemove = 
        {
            //typeof(EarlyUpdate.Physics2DEarlyUpdate),
            // Physics 2D
#if UNITY_2022_2_OR_NEWER
            typeof(EarlyUpdate.Physics2DEarlyUpdate),
#endif
            typeof(FixedUpdate.Physics2DFixedUpdate),
            typeof(PreUpdate.Physics2DUpdate),
            typeof(PreLateUpdate.Physics2DLateUpdate),
 
            // Wind
            typeof(PreUpdate.WindUpdate),
 
            // Cloth
            typeof(PostLateUpdate.PhysicsSkinnedClothBeginUpdate),
            typeof(PostLateUpdate.PhysicsSkinnedClothFinishUpdate),
   
            // Old network system
            typeof(PreLateUpdate.UpdateNetworkManager),
        
            // Other
            typeof(Initialization.UpdateCameraMotionVectors),
            typeof(Initialization.XREarlyUpdate),
            typeof(Initialization.AsyncUploadTimeSlicedUpdate),
            typeof(EarlyUpdate.PresentBeforeUpdate),
            typeof(EarlyUpdate.PollPlayerConnection),
            typeof(EarlyUpdate.GpuTimestamp),
            typeof(EarlyUpdate.AnalyticsCoreStatsUpdate),
            typeof(EarlyUpdate.UnityWebRequestUpdate),
            typeof(EarlyUpdate.ExecuteMainThreadJobs),
            typeof(EarlyUpdate.ProcessMouseInWindow),
            typeof(PreUpdate.NewInputUpdate),
            typeof(PreUpdate.UpdateVideo),
            typeof(PreUpdate.CheckTexFieldInput),
            typeof(EarlyUpdate.XRUpdate),
            typeof(EarlyUpdate.PerformanceAnalyticsUpdate),
            typeof(PostLateUpdate.UpdateCustomRenderTextures),
            typeof(PostLateUpdate.XRPostLateUpdate),
            typeof(PostLateUpdate.UpdateVideo),
            typeof(PostLateUpdate.UpdateVideoTextures),
            typeof(PostLateUpdate.XRPreEndFrame),
            
            /*typeof(PreUpdate.SendMouseEvents),
            
            typeof(EarlyUpdate.ClearIntermediateRenderers),
            typeof(EarlyUpdate.ClearLines),
            typeof(EarlyUpdate.ResetFrameStatsAfterPresent),
            typeof(EarlyUpdate.UpdateAsyncInstantiate),
            typeof(EarlyUpdate.UpdateAsyncReadbackManager),
            typeof(EarlyUpdate.UpdateStreamingManager),
            typeof(EarlyUpdate.UpdateTextureStreamingManager),
            typeof(EarlyUpdate.UpdatePreloading),
            typeof(EarlyUpdate.UpdateContentLoading),
            typeof(EarlyUpdate.RendererNotifyInvisible),
            typeof(EarlyUpdate.PlayerCleanupCachedData),
            typeof(EarlyUpdate.UpdateMainGameViewRect),
            typeof(EarlyUpdate.UpdateCanvasRectTransform),
            typeof(EarlyUpdate.ProcessRemoteInput),
            typeof(EarlyUpdate.ScriptRunDelayedStartupFrame),
            typeof(EarlyUpdate.SpriteAtlasManagerUpdate),
        
            typeof(PreUpdate.IMGUISendQueuedEvents),
        
            typeof(Update.ScriptRunDelayedTasks),
            typeof(Update.ScriptRunDelayedDynamicFrameRate),
        
            typeof(PreLateUpdate.LegacyAnimationUpdate),
            typeof(PreLateUpdate.ConstraintManagerUpdate),
            typeof(PreLateUpdate.ParticleSystemBeginUpdateAll),
            typeof(PreLateUpdate.ScriptRunBehaviourLateUpdate),
            typeof(PreLateUpdate.EndGraphicsJobsAfterScriptUpdate),
        
            typeof(PostLateUpdate.UpdateAllRenderers),
            typeof(PostLateUpdate.EndGraphicsJobsAfterScriptLateUpdate),
            typeof(PostLateUpdate.ScriptRunDelayedDynamicFrameRate),
            typeof(PostLateUpdate.UpdateRectTransform),
            typeof(PostLateUpdate.PlayerUpdateCanvases),
            typeof(PostLateUpdate.UpdateAudio),
            typeof(PostLateUpdate.VFXUpdate),
            typeof(PostLateUpdate.ParticleSystemEndUpdateAll),
            
            typeof(PostLateUpdate.ResetInputAxis),
            typeof(PostLateUpdate.ShaderHandleErrors),
            typeof(PostLateUpdate.GUIClearEvents),
            typeof(PostLateUpdate.TriggerEndOfFrameCallbacks),
            typeof(PostLateUpdate.InputEndFrame),
            typeof(PostLateUpdate.UpdateCaptureScreenshot),
            typeof(PostLateUpdate.BatchModeUpdate),
            typeof(PostLateUpdate.SortingGroupsUpdate),
            typeof(PostLateUpdate.UpdateAllSkinnedMeshes),
            typeof(PostLateUpdate.MemoryFrameMaintenance),
            typeof(PostLateUpdate.UpdateResolution),
            typeof(PostLateUpdate.EnlightenRuntimeUpdate),
            typeof(PostLateUpdate.PlayerEmitCanvasGeometry),
            typeof(PostLateUpdate.PlayerSendFrameStarted),
            typeof(PostLateUpdate.PlayerSendFrameComplete),
            typeof(PostLateUpdate.PlayerSendFramePostPresent)*/
        
        
        };
 
        private static readonly string[] TypesToRemoveAsString = new string[] 
        {
            
            "UnityEngine.PlayerLoop.FixedUpdate+PhysicsClothFixedUpdate",
            "UnityEngine.PlayerLoop.PreUpdate+PhysicsClothUpdate"
        };
 
 
        [RuntimeInitializeOnLoadMethod]
        private static void RemoveUnusedPackageFromPlayerLoop()
        {
            // var playerLoop = PlayerLoop.GetCurrentPlayerLoop();
            //
            // foreach(var type in TypesToRemove)
            // {
            //     TryRemoveTypeFrom(ref playerLoop, type);
            // }
            // foreach (var type in TypesToRemoveAsString) 
            // {
            //     TryRemoveTypeFrom(ref playerLoop, type);
            // }
            //
            // PlayerLoop.SetPlayerLoop(playerLoop);
        }
 
        private static void DisplayRecursivly(int level, PlayerLoopSystem system)
        {
            AKDebug.Log(level + " " + system.type);
            if (system.subSystemList == null) return;
            
            AKDebug.Log(level + " subSystemList.Length: " + system.subSystemList.Length);
            foreach (var sys in system.subSystemList)
            {
                DisplayRecursivly(level + 1, sys);
            }
        }
        
        private static bool TryRemoveTypeFrom(ref PlayerLoopSystem currentSystem, Type type)
        {
            var subSystems = currentSystem.subSystemList;
            if (subSystems == null)
            {
                return false;
            }
 
            for (int i = 0; i < subSystems.Length; i++)
            {
                if (subSystems[i].type == type)
                {
                    var newSubSystems = new PlayerLoopSystem[subSystems.Length - 1];
 
                    Array.Copy(subSystems, newSubSystems, i);
                    Array.Copy(subSystems, i + 1, newSubSystems, i, subSystems.Length - i - 1);
 
                    currentSystem.subSystemList = newSubSystems;
 
                    return true;
                }
 
                if (TryRemoveTypeFrom(ref subSystems[i], type))
                {
                    return true;
                }
            }
 
            return false;
        }
 
        private static bool TryRemoveTypeFrom(ref PlayerLoopSystem currentSystem, string type)
        {
            var subSystems = currentSystem.subSystemList;
            if (subSystems == null) {
                return false;
            }
 
            for (int i = 0; i < subSystems.Length; i++) 
            {
                if (subSystems[i].type.ToString() == type)
                {
                    var newSubSystems = new PlayerLoopSystem[subSystems.Length - 1];
 
                    Array.Copy(subSystems, newSubSystems, i);
                    Array.Copy(subSystems, i + 1, newSubSystems, i, subSystems.Length - i - 1);
 
                    currentSystem.subSystemList = newSubSystems;
 
                    return true;
                }
 
                if (TryRemoveTypeFrom(ref subSystems[i], type))
                {
                    return true;
                }
            }
 
            return false;
        }
    }
}
 