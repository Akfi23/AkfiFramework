using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using _Source.Code._AKFramework.AKCore.Editor;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public sealed class AnimatorHashTool : IAKEditorTool
{
    private const string characters_template = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
        private const string numbers_template = "0123456789";
        private const string symbols_template = ".-_$#@*()[]{}+:?!&',^=<>~`";
        private const int max_characters = 256;
        
        [Button]
        private static void GenerateAnimatorParametersHashes()
        {
            EditorUtility.DisplayProgressBar("Scripts Generation", "Wait a second friend...", 0);

            var guids = AssetDatabase.FindAssets($"t:{nameof(AnimatorController)}");

            var parameters = new List<StringBuilder>();
            foreach (var guid in guids)
            {
                var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(AssetDatabase.GUIDToAssetPath(guid));
                AddParameters(controller, ref parameters);
            }
            
            Generate(in parameters);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            EditorUtility.ClearProgressBar();
        }

        private static void AddParameters(AnimatorController controller, ref List<StringBuilder> parameters)
        {
            foreach (var property in controller.parameters)
            {
                var propertyName = formatPropertyName(property.name.Replace("/", "__"));

                if (string.IsNullOrWhiteSpace(propertyName)) continue;

                var line = $"{indents(2)}public static readonly int {propertyName} = {property.nameHash};";
                if (!parameters.Exists(x => x.Equals(line)))
                {
                    parameters.Add(new StringBuilder(line));
                }
            }
        }
        
        private static void Generate(in List<StringBuilder> parameters)
        {
            var sbTrans = new StringBuilder();

            sbTrans.AppendLine("namespace AKFramework.Generated");
            sbTrans.AppendLine("{");
            sbTrans.AppendLine($"{indents(1)}public static class AnimatorHashStrings"); //Filename
            sbTrans.AppendLine($"{indents(1)}{{");
            foreach (var str in parameters)
            {
                sbTrans.AppendLine(str.ToString());
            }

            sbTrans.AppendLine($"{indents(1)}}}");
            sbTrans.AppendLine("}");
            
            var generatedScriptFilePath = getPathToGeneratedFile("AnimatorHashStrings");

            var filePath = Application.dataPath + "/" + generatedScriptFilePath;
            Debug.Log(filePath);
            var fileText = sbTrans.ToString();

            var dirPath = new FileInfo(filePath).DirectoryName;

            if (dirPath != null && !Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            
            File.WriteAllText(filePath, fileText, Encoding.UTF8);
        }
        
        private static string getPathToGeneratedFile(string fileName)
        {
            var settings = AKCoreProjectSettings.GetOrCreateSettings();
            return $"{settings.GeneratorScriptsPath}/{fileName}.cs";
        }
        
        private static string indents(int amount)
        {
            var result = string.Empty;

            for (int i = 0; i < amount; i++)
            {
                result += "    ";
            }

            return result;
        }
        
        private static string formatPropertyName(string property, bool allowFullLength = false)
        {
            property = getValidPropertyName(property);

            if (string.IsNullOrEmpty(property))
                return string.Empty;

            // C# IDs can't start with a number
            if (numbers_template.IndexOf(property[0]) >= 0)
                property = "_" + property;

            if (!allowFullLength && property.Length > max_characters)
                property = property.Substring(0, max_characters);

            // Remove invalid characters
            var chars = property.ToCharArray();
            for (int i = 0, imax = chars.Length; i < imax; ++i)
                if (characters_template.IndexOf(chars[i]) < 0)
                    chars[i] = '_';
            return new string(chars);
        }
        
        private static string getValidPropertyName(string text, bool allowCategory = false)
        {
            if (text == null)
                return null;
            return removeNonASCIICharacters(text, allowCategory);
        }

        private static string removeNonASCIICharacters(string text, bool allowCategory = false)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            // Remove Non-Letter/Digits and collapse all extra espaces into a single space
            var current = 0;
            var output = new char[text.Length];
            var skipped = false;

            foreach (var cc in text.Trim().ToCharArray())
            {
                var c = ' ';
                if ((allowCategory && (cc == '\\' || cc == '\"' || (cc == '/'))) ||
                    char.IsLetterOrDigit(cc) ||
                    symbols_template.IndexOf(cc) >= 0)
                {
                    c = cc;
                }

                if (char.IsWhiteSpace(c))
                {
                    if (!skipped)
                    {
                        if (current > 0)
                            output[current++] = ' ';

                        skipped = true;
                    }
                }
                else
                {
                    skipped = false;
                    output[current++] = c;
                }
            }

            return new string(output, 0, current);
        }

        public string Title => "Animator";
}
