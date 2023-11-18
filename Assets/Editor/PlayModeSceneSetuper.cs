using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class PlayModeSceneSetuper
{
    private static string lastOpenedScenePath
    {
        get => EditorPrefs.GetString($"{Application.identifier}_LastOpenedScenePath", string.Empty);
        set => EditorPrefs.SetString($"{Application.identifier}_LastOpenedScenePath", value);
    }

    private static readonly string contextScenePath = "Assets/_Source/Scenes/core/scene_context.unity";

    static PlayModeSceneSetuper()
    {
        EditorSceneManager.activeSceneChangedInEditMode += (previousScene, newScene) =>
        {
            if (Application.isPlaying) return;

            if (newScene == EditorSceneManager.GetSceneByBuildIndex(0))
            {
                EditorSceneManager.playModeStartScene = null;
                lastOpenedScenePath = string.Empty;
            }
            else if (newScene.path == contextScenePath)
            {
                EditorSceneManager.playModeStartScene = null;
                lastOpenedScenePath = string.Empty;
            }
            else
            {
                var contextSceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(contextScenePath);
                EditorSceneManager.playModeStartScene = contextSceneAsset;
                lastOpenedScenePath = newScene.path;
            }
        };
    }
}
