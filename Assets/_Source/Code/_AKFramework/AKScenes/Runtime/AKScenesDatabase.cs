using System.Collections.Generic;
using UnityEngine;
using System;
using _Source.Code._AKFramework.AKCore.Runtime;
using Sirenix.OdinInspector;

namespace _Source.Code._AKFramework.AKScenes.Runtime
{
    [CreateAssetMenu(menuName = "AKFramework/Scenes Database")]

    public class AKScenesDatabase : AKDatabase
    {
        public AKScenesGroupContainer[] Groups => _groups;
        
        [SerializeField]
        private AKScenesGroupContainer[] _groups;

        public override string Title => "Scenes";
        
#if UNITY_EDITOR

        public override void UpdateAKType()
        {
            foreach (var group in Groups)
            {
                foreach (var scene in group.Scenes)
                {
                    AKTypeUtilsUpdater.AddID($"{group._Name}/{scene._Name}", scene._Id);
                }
                
                AKTypeUtilsUpdater.AddID($"{group._Name}", group._Id);
            }
        }

        [Button]
        private void UpdateAKScene()
        {
            UpdateAKType();
            AKTypeUtilsUpdater.UpdateID();
        }
#endif

        protected override void Generate(out AKGenerationData[] generationData)
        {
            var groups = new Dictionary<int, string>();
            var scenes = new Dictionary<int, string>();

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