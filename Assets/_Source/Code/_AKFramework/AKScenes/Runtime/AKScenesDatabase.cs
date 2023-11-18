using System.Collections.Generic;
using UnityEngine;
using System;

namespace _Source.Code._AKFramework.AKScenes.Runtime
{
    [CreateAssetMenu(menuName = "AKFramework/Scenes Database")]

    public class AKScenesDatabase : AKDatabase
    {
        public AKScenesGroupContainer[] Groups => _groups;
        
        [SerializeField]
        private AKScenesGroupContainer[] _groups;

        public override string Title => "Scenes";

        protected override void Generate(out AKGenerationData[] generationData)
        {
            var groups = new Dictionary<string, string>();
            var scenes = new Dictionary<string, string>();

            foreach (var layer0 in _groups)
            {
                groups[layer0._Id] = $"{layer0._Name}";
                foreach (var layer1 in layer0.Scenes)
                {
                    scenes[layer1._Id] = $"{layer0._Name}/{layer1._Name}";
                }
            }

            generationData = new[]
            {
                new AKGenerationData
                {
                    FileName = "AKScenes",
                    Usings = new[]
                    {
                        "using _Source.Code._AKFramework.AKScenes.Runtime;"
                    },
                    AKType = typeof(AKScene),
                    Properties = scenes
                },
                new AKGenerationData
                {
                    FileName = "AKScenesGroups",
                    Usings = new string[]
                    {
                        "using _Source.Code._AKFramework.AKScenes.Runtime;"
                    },
                    AKType = typeof(AKScenesGroup),
                    Properties = groups
                }
            };
        }
    }
}