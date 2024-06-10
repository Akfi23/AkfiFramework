using System.Linq;
using _Source.Code._AKFramework.AKScenes.Runtime;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor
{
    public class SelectSceneEditorWindow : EditorWindow
    {
        private static SelectSceneEditorWindow _window;
        private static bool _isSceneDatabaseMode = true;
        
        private static string[] _sceneArray = { };
        private Vector2 _scrollPos;
        
        [MenuItem("Window/Select Scene")]
        public static void OpenWindow()
        {
            if (_window != null)
            {
                _window.Close();
            }
            _window = GetWindow<SelectSceneEditorWindow>();
            _window.titleContent = new GUIContent("Select Scene");
            RefreshSceneArray();
            _isSceneDatabaseMode = EditorPrefs.GetBool("isSceneDatabaseMode", true);
            _window.Show();
        }
        
        [UnityEditor.Callbacks.DidReloadScripts]
        static void RefreshSceneArray()
        {
            _sceneArray = (from scene in EditorBuildSettings.scenes select scene.path).ToArray();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal("Box");
            EditorGUILayout.LabelField("Is Scene Database");
            var newSceneDatabaseModeValue = EditorGUILayout.Toggle(_isSceneDatabaseMode);
            if (newSceneDatabaseModeValue != _isSceneDatabaseMode)
            {
                EditorPrefs.SetBool("isSceneDatabaseMode", newSceneDatabaseModeValue);
                _isSceneDatabaseMode = newSceneDatabaseModeValue;
            }
            EditorGUILayout.EndHorizontal();

            if (_isSceneDatabaseMode)
            {
                var assets = AssetDatabase.FindAssets($"t:{typeof(AKScenesDatabase)}");
                if (assets.Length <= 0)
                {
                    _window.Close();
                    return;
                }

                AKScenesDatabase database = null;

                foreach (var asset in assets)
                {
                    var path = AssetDatabase.GUIDToAssetPath(asset);
                    database = AssetDatabase.LoadAssetAtPath<AKScenesDatabase>(path);
                    break;
                }

                if (database == null)
                {
                    _window.Close();
                    return;
                }
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, false, false);
                foreach (var group in database.Groups)
                {
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.LabelField(group._Name);
                    foreach (var scene in group.Scenes)
                    {
                        if (GUILayout.Button(scene._Name))
                        {
                            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                            {
                                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                                EditorSceneManager.OpenScene(scene.Scene);
                                _window.Close();
                            }
                            else
                            {
                                EditorSceneManager.OpenScene(scene.Scene);
                                _window.Close();
                            }
                        }
                    }

                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Separator();
                }
                GUILayout.EndScrollView();
            }
            else
            {
                GUILayout.BeginVertical("Box");

                if (_sceneArray.Length > 0)
                {
                    _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, false, false);
                    for (int i = 0; i < _sceneArray.Length; i++)
                    {
                        if (GUILayout.Button(_sceneArray[i].Substring(_sceneArray[i].LastIndexOf('/') + 1)))
                        {
                            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                            {
                                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                                EditorSceneManager.OpenScene(_sceneArray[i]);
                                _window.Close();
                            }
                            else
                            {
                                EditorSceneManager.OpenScene(_sceneArray[i]);
                                _window.Close();
                            }
                        }
                    }
                    GUILayout.EndScrollView();
                }
                else
                {
                    GUILayout.Label("There are no scenes. Please check your Build Settings.");
                }

                GUILayout.EndVertical();
 
                GUILayout.Space(2f);

           

                GUILayout.BeginHorizontal("Box");

                if (GUILayout.Button("Reload Scene List"))
                {
                    RefreshSceneArray();
                }

                GUILayout.EndHorizontal();
            }
        }
    }
}