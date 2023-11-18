using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace Inspector.ScriptsTemplates
{
    public class ScriptTemplateGenerator : ScriptableObject
    {
        const string Title = "Script template generator";

        const string SystemTemplate = "System.cs.txt";
        const string ComponentTemplate = "Component.cs.txt";

        private static void CreateEmptyFolder (string folderPath)
        {
            if (!Directory.Exists (folderPath)) {
                try {
                    Directory.CreateDirectory (folderPath);
                    File.Create ($"{folderPath}/.gitkeep");
                } catch {
                    // ignored
                }
            }
        }

        [MenuItem ("Assets/Create/ECS/Create Component", false, -199)]
        private static void CreateComponentTpl()
        {
            const string fileName = "/NewComponent.cs";
            CreateAndRenameAsset ($"{GetAssetPath ()}{fileName}", GetIcon (),
                (name) => CreateTemplateInternal (GetTemplateContent (ComponentTemplate), name, fileName));
        }
        
        [MenuItem ("Assets/Create/ECS/Create System", false, -198)]
        private static void CreateSystemTpl()
        {
            const string fileName = "/NewSystem.cs";
            CreateAndRenameAsset ($"{GetAssetPath ()}{fileName}", GetIcon (),
                (name) => CreateTemplateInternal (GetTemplateContent (SystemTemplate), name, fileName));
        }

        public static string CreateTemplate (string proto, string fileName, string name)
        {
            if (string.IsNullOrEmpty (fileName)) {
                return "Invalid filename";
            }

            var assetPath = AssetDatabase.GenerateUniqueAssetPath(fileName);
            var ns = assetPath.Replace(name, "").Replace('/', '.').Replace("Assets.", "").Trim();
            proto = proto.Replace ("#NS#", ns);
            proto = proto.Replace ("#SCRIPTNAME#", SanitizeClassName (Path.GetFileNameWithoutExtension (fileName)));
            try {
                
                File.WriteAllText (assetPath, proto);
            } catch (Exception ex) {
                return ex.Message;
            }
            AssetDatabase.Refresh ();
            return null;
        }

        static string SanitizeClassName (string className)
        {
            var sb = new StringBuilder ();
            var needUp = true;
            foreach (var c in className) {
                if (char.IsLetterOrDigit (c)) {
                    sb.Append (needUp ? char.ToUpperInvariant (c) : c);
                    needUp = false;
                } else {
                    needUp = true;
                }
            }
            return sb.ToString ();
        }

        static string CreateTemplateInternal (string proto, string fileName, string name)
        {
            var res = CreateTemplate (proto, fileName, name);
            if (res != null) {
                EditorUtility.DisplayDialog (Title, res, "Close");
            }
            return res;
        }

        static string GetTemplateContent (string proto) 
        {
            // hack: its only one way to get current editor script path. :(
            var pathHelper = CreateInstance<ScriptTemplateGenerator> ();
            var path = Path.GetDirectoryName (AssetDatabase.GetAssetPath (MonoScript.FromScriptableObject (pathHelper)));
            DestroyImmediate (pathHelper);
            try {
                return File.ReadAllText (Path.Combine (path ?? "", proto));
            } catch {
                return null;
            }
        }

        static string GetAssetPath ()
        {
            var path = AssetDatabase.GetAssetPath (Selection.activeObject);
            if (!string.IsNullOrEmpty (path) && AssetDatabase.Contains (Selection.activeObject)) {
                if (!AssetDatabase.IsValidFolder (path)) {
                    path = Path.GetDirectoryName (path);
                }
            } else {
                path = "Assets";
            }
            return path;
        }

        static Texture2D GetIcon () 
        {
            return EditorGUIUtility.IconContent ("cs Script Icon").image as Texture2D;
        }

        static void CreateAndRenameAsset (string fileName, Texture2D icon, Action<string> onSuccess) 
        {
            var action = CreateInstance<CustomEndNameAction> ();
            action.Callback = onSuccess;
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists (0, action, fileName, icon, null);
        }

        sealed class CustomEndNameAction : EndNameEditAction 
        {
            [NonSerialized] public Action<string> Callback;

            public override void Action (int instanceId, string pathName, string resourceFile) {
                Callback?.Invoke (pathName);
            }
        }
    }
}
