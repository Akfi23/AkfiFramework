using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Source.Code._AKFramework.AKUI.Runtime
{
    [CreateAssetMenu(menuName = "AKFramework/UI Database")]
    public sealed class AKUIDatabase : AKDatabase
    {
        public AKScreenGroupContainer[] ScreenGroupsContainers => screenGroupsContainers;
        
        [Title("UI Database", TitleAlignment = TitleAlignments.Centered)]
        [SerializeField]
        private AKScreenGroupContainer[] screenGroupsContainers;

        public override string Title => "UI";

        protected override void Generate(out AKGenerationData[] generationData)
        {
            var screens = new Dictionary<string, string>();

            foreach (var layer0 in screenGroupsContainers)
            {
                foreach (var layer1 in layer0.ScreenContainers)
                {
                    screens[layer1._Id] = $"{layer0._Name}/{layer1._Name}";
                }
            }

            var buttons = new Dictionary<string, string>();

            foreach (var layer0 in screenGroupsContainers)
            {
                foreach (var layer1 in layer0.ScreenContainers)
                {
                    foreach (var layer2 in layer1.ButtonContainers)
                    {
                        buttons[layer2._Id] = $"{layer0._Name}/{layer1._Name}/{layer2._Name}";
                    }
                }
            }
            
            var toggles = new Dictionary<string, string>();

            foreach (var layer0 in screenGroupsContainers)
            {
                foreach (var layer1 in layer0.ScreenContainers)
                {
                    foreach (var layer2 in layer1.ToggleContainers)
                    {
                        toggles[layer2._Id] = $"{layer0._Name}/{layer1._Name}/{layer2._Name}";
                    }
                }
            }

            generationData = new[]
            {
                new AKGenerationData
                {
                    FileName = "AKScreens",
                    Usings = new[]
                    {
                        "using _Source.Code._AKFramework.AKUI.Runtime;",
                    },
                    AKType = typeof(AKScreen),
                    Properties = screens
                },
                new AKGenerationData
                {
                    FileName = "AKButtons",
                    Usings = new[]
                    {
                        "using _Source.Code._AKFramework.AKUI.Runtime;",
                    },
                    AKType = typeof(AKButton),
                    Properties = buttons
                },
                new AKGenerationData
                {
                    FileName = "AKToggles",
                    Usings = new[]
                    {
                        "using _Source.Code._AKFramework.AKUI.Runtime;",
                    },
                    AKType = typeof(AKToggle),
                    Properties = toggles
                }
            };
        }
    }
}