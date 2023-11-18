using _Source.Code._AKFramework.AKCore.Editor;
using Sirenix.OdinInspector;

namespace _Source.Code._AKFramework.AKECS.Editor
{
    public sealed class AKECSLiteTool : IAKEditorTool
    {
        [Button]
        private static void GenerateAuthorings()
        {
            AKComponentsGenerator.Generate(true);
        }

        public string Title => "ECS Lite";
    }
}