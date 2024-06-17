using System.Collections.Generic;
using _Source.Code._AKFramework.AKCore.Runtime;
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
        
#if UNITY_EDITOR

        public override void UpdateAKType()
        {
            foreach (var group in Groups)
            {
                foreach (var tag in group.Tags)
                {
                    AKTypeUtilsUpdater.AddID($"{group._Name}/{tag._Name}", tag._Id);
                }
                
                AKTypeUtilsUpdater.AddID($"{group._Name}", group._Id);
            }
        }

        [Button]
        private void UpdateAKTag()
        {
            UpdateAKType();
            AKTypeUtilsUpdater.UpdateID();
        }
#endif

        protected override void Generate(out AKGenerationData[] generationData)
        {
            var groups = new Dictionary<int, string>();
            var tags = new Dictionary<int, string>();

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