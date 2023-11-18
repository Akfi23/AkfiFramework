using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Source.Code._AKFramework.AKTags.Runtime
{
    [CreateAssetMenu(menuName = "AKFramework/Tags Database")]
    public class AKTagsDatabase : AKDatabase
    {
        public AKTagsGroupContainer[] Groups => _groups;
        
        [Title("Tags Database", TitleAlignment = TitleAlignments.Centered)]
        [SerializeField]
        private AKTagsGroupContainer[] _groups;

        public override string Title => "Tags";

        protected override void Generate(out AKGenerationData[] generationData)
        {
            var groups = new Dictionary<string, string>();
            var tags = new Dictionary<string, string>();

            foreach (var layer0 in _groups)
            {
                groups[layer0._Id] = $"{layer0._Name}";
                foreach (var layer1 in layer0.Tags)
                {
                    tags[layer1._Id] = $"{layer0._Name}/{layer1._Name}";
                }
            }

            generationData = new[]
            {
                new AKGenerationData
                {
                    FileName = "AKTags",
                    Usings = new[]
                    {
                        "using _Source.Code._AKFramework.AKTags.Runtime;"
                    },
                    AKType = typeof(AKTag),
                    Properties = tags
                },
                new AKGenerationData
                {
                    FileName = "AKTagsGroups",
                    Usings = new string[]
                    {
                        "using _Source.Code._AKFramework.AKTags.Runtime;"
                    },
                    AKType = typeof(AKTagsGroup),
                    Properties = groups
                }
            };
        }
    }
}