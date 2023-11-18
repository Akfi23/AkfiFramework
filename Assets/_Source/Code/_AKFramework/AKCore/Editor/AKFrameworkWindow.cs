using System;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace _Source.Code._AKFramework.AKCore.Editor
{
    public class AKFrameworkWindow : OdinMenuEditorWindow
    {
        [MenuItem("Window/AKFramework")]
        private static void OpenWindow()
        {
            var window = GetWindow<AKFrameworkWindow>();
            window.minSize = new Vector2(300f, 300f);
            window.titleContent = new GUIContent("AKFramework");
            window.Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(false)
            {
                Config =
                {
                    DrawSearchToolbar = true
                },
                DefaultMenuStyle = OdinMenuStyle.TreeViewStyle
            };


            var databases = AKEditorExtensions.FindAssets<AKDatabase>();

            for (int i = 0; i < databases.Count; i++)
            {
                tree.Add($"Databases/{databases[i].Title}", databases[i]);
            }
            
            var tools = GetInheritedClasses(typeof(IAKEditorTool));

            foreach (var tool in tools)
            {
                var instance = Activator.CreateInstance(tool) as IAKEditorTool;
                tree.Add($"Tools/{instance?.Title}", instance);
            }
            
            return tree;
        }

        private Type[] GetInheritedClasses(Type MyType) 
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && MyType.IsAssignableFrom(t))
                .ToArray();
        }
    }
}
