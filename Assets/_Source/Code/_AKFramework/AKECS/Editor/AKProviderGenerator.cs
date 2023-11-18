using System;
using System.IO;
using System.Linq;
using System.Reflection;
using _Source.Code._AKFramework.AKECS.Runtime;
using UnityEditor;
using UnityEngine;

namespace _Source.Code._Core.ECS.Generator
{
    [InitializeOnLoad]
    public static class AKProviderGenerator
    {
        private static string providerFileTemplate =
            @"using UnityEngine;
using @@COMPONENTNAMESPACE@@;

namespace @@NAMESPACE@@
{
    [DisallowMultipleComponent]
    public sealed class @@COMPONENTNAME@@Provider : AKProvider<@@COMPONENTNAME@@> {}
}
";
        
        static AKProviderGenerator()
        {
            var settings = AssetDatabase.LoadAssetAtPath<AKECSProjectSettings>(AKECSProjectSettings.settingsPath);
            
            if(!settings.GenerateAuthorings) return;

            var authoringsToGenerate = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsValueType && t.GetCustomAttribute<AKGenerateProviderAttribute>() != null)
                .ToList();

            foreach (var type in authoringsToGenerate)
            {
                var providerClassName = $"{type.Name}Authoring";
                var providerFileName = $"{providerClassName}.cs";

                if (File.Exists(Path
                    .GetFullPath(Path.Combine(
                        Application.dataPath + Path.DirectorySeparatorChar + settings.AuthoringsFilePath,
                        providerFileName))))
                {
                    continue;
                }

                var fileContent = providerFileTemplate
                    .Replace("@@COMPONENTNAMESPACE@@", settings.ComponentsNamespace)
                    .Replace("@@NAMESPACE@@", settings.AuthoringsNamespace)
                    .Replace("@@COMPONENTNAME@@", type.Name)
                    .Replace("@@COMPONENTNAME@@", type.Name);

                File.WriteAllText(
                    Path.GetFullPath(Path.Combine(
                        Application.dataPath + Path.DirectorySeparatorChar + settings.AuthoringsFilePath, providerFileName)),
                    fileContent);
            }

            AssetDatabase.Refresh();
        }
    }
}