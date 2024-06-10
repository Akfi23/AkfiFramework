using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

namespace Editor
{
    [InitializeOnLoad]
    public class EditorClearPrefsButton
    {
        static class ToolbarStyles
        {
            public static readonly GUIStyle CommandButtonStyle;

            static ToolbarStyles()
            {
                CommandButtonStyle = new GUIStyle("Command")
                {
                    fontSize = 15,
                    alignment = TextAnchor.MiddleCenter,
                    imagePosition = ImagePosition.TextOnly,
                    fontStyle = FontStyle.Bold,
                    fixedWidth = 100f
                };
            }
        }

        static EditorClearPrefsButton()
        {
            ToolbarExtender.LeftToolbarGUI.Add(LeftToolbarGUI);
        }

        private static void LeftToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            GUI.enabled = !EditorApplication.isPlaying;

            GUI.backgroundColor = Color.magenta;

            if (GUILayout.Button(new GUIContent("Clear prefs", "Clear Prefs"), ToolbarStyles.CommandButtonStyle, GUILayout.Width(500f)))
            {
                PlayerPrefs.DeleteAll();
            }

            GUI.backgroundColor = Color.white;
            GUI.enabled = true;
        }
    }
}