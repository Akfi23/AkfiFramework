using System;
using _Source.Code._AKFramework.AKTags.Runtime;

namespace _Source.Code.Objects
{
    [Serializable]
    public class ItemData
    {
        [AKTagsGroup("Items")] 
        public AKTag ItemTag;
        public int Value;

        public ItemData(AKTag itemTag, int value)
        {
            ItemTag = itemTag;
            Value = value;
        }

        public override string ToString()
        {
            return $"Tag: {ItemTag._Name} Value: {Value}";
        }
    }
}