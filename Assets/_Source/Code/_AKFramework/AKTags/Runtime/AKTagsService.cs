using System.Collections.Generic;
using Zenject;

namespace _Source.Code._AKFramework.AKTags.Runtime
{
    public class AKTagsService : IAKTagsService
    {
        private readonly Dictionary<AKTagsGroup, AKTag[]> _tagsMap = new();

        private AKTagsDatabase _database;

        [Inject]
        private void Init(AKTagsDatabase database)
        {
            _database = database;

            foreach (var groupContainer in _database.Groups)
            {
                var akTags = new AKTag[groupContainer.Tags.Length];
                for(int i = 0; i < groupContainer.Tags.Length; i++)
                {
                    akTags[i] = new AKTag(groupContainer.Tags[i]._Id, $"{groupContainer._Name}/{groupContainer.Tags[i]._Name}");
                }
                _tagsMap[new AKTagsGroup(groupContainer._Id, groupContainer._Name)] = akTags;
            }
        }
        
        public AKTag GetAKTagByName(string name)
        {
            foreach (var maps in _tagsMap)
            {
                foreach (var tag in maps.Value)
                {
                    if (tag._Name.Equals(name)) return tag;
                }
            }

            return null;
        }

        public AKTag GetAKTagByID(string id)
        {
            foreach (var maps in _tagsMap)
            {
                foreach (var tag in maps.Value)
                {
                    if (tag._Id.Equals(id)) return tag;
                }
            }

            return null;
        }

        public AKTagsGroup GetAKTagsGroup(AKTag tag)
        {
            foreach (var key in _tagsMap.Keys)
            {
                if (_tagsMap[key].HasTag(tag)) return key;
            }

            return null;
        }
    }
}