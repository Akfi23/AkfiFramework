#if ECS_EXIST
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using _Source.Code._AKFramework.AKCore.Editor;
using _Source.Code._AKFramework.AKPools.Runtime;
using UnityEditor;
using UnityEngine;

namespace _Source.Code._AKFramework.AKPools.Editor
{
    [InitializeOnLoad]
    public static class AKAuthoringPoolResetGenerator
    {
        private static string providerFileTemplate =
            @"_Source.Code._AKFramework.AKECS.Runtime;
using @@COMPONENTNAMESPACE@@;
using Leopotam.EcsLite;
using _Source.Code._AKFramework.AKPools.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace @@NAMESPACE@@
{
    [DisallowMultipleComponent, AddComponentMenu(""AKFramework/ECS/Components/@@NAME@@""), HideMonoScript]
    public sealed class _@@COMPONENTNAME@@ : AKComponent<@@COMPONENTNAME@@>, IAKPoolReset 
    {
        public void Reset()
        {
            if (PackedEntity.Unpack(World, out var entity))
            {
                World.GetPool<@@COMPONENTNAME@@>().Get(entity) = Value;
            }
        }
    }
}
";

        static AKAuthoringPoolResetGenerator()
        {
            Generate();
        }

        public static void Generate(bool force = false)
        {
            var settings = AssetDatabase.LoadAssetAtPath<AKCoreProjectSettings>(AKCoreProjectSettings._assetPath);

            var authoringsToGenerate = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsValueType && t.GetCustomAttribute<AKGenerateAuthoringPoolResetAttribute>() != null)
                .ToList();

            foreach (var type in authoringsToGenerate)
            {
                var providerClassName = $"_{type.Name}";
                var providerFileName = $"{providerClassName}.cs";

                if (!force)
                {
                    if (File.Exists(Path
                            .GetFullPath(Path.Combine(
                                Application.dataPath + Path.DirectorySeparatorChar + settings.GeneratorScriptsPath,
                                providerFileName))))
                    {
                        continue;
                    }

                }
                
                var fileContent = providerFileTemplate
                    .Replace("@@COMPONENTNAMESPACE@@", type.Namespace)
                    .Replace("@@NAMESPACE@@", "AKFramework.Generated")
                    .Replace("@@COMPONENTNAME@@", type.Name)
                    .Replace("@@NAME@@", AddSpacesToSentence(type.Name).Replace("Ref", "Reference"));

                File.WriteAllText(
                    Path.GetFullPath(Path.Combine(
                        Application.dataPath + Path.DirectorySeparatorChar + settings.GeneratorScriptsPath,
                        providerFileName)),
                    fileContent);
            }

            AssetDatabase.Refresh();
        }

        static string AddSpacesToSentence(string text, bool preserveAcronyms = true)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            var newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }

            return newText.ToString();
        }
    }
}
#endif