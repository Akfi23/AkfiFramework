namespace _Source.Code._AKFramework.AKTags.Runtime
{
    public static partial class AKExtensions
    {
        public static bool HasTag(this AKTag tag, AKTag[] otherTags)
        {
            if (tag == null) return false;
            if (otherTags == null || otherTags.Length <= 0) return false;

            for (int j = 0; j < otherTags.Length; j++)
            {
                if (tag.HasTag(otherTags[j])) return true;
            }

            return false;
        }

        public static bool HasTag(this AKTag tag, AKTag otherTag)
        {
            if (tag == null) return false;

            return tag._Id == otherTag._Id;
        }

        public static bool HasTag(this AKTag[] tags, AKTag[] otherTags)
        {
            if (tags == null || tags.Length <= 0) return false;
            if (otherTags == null || otherTags.Length <= 0) return false;

            for (int i = 0; i < tags.Length; i++)
            {
                for (int j = 0; j < otherTags.Length; j++)
                {
                    if (tags[i].HasTag(otherTags[j])) return true;
                }
            }

            return false;
        }

        public static bool HasTag(this AKTag[] tags, AKTag otherTag)
        {
            if (tags == null || tags.Length <= 0) return false;

            foreach (var t in tags)
            {
                if (t.HasTag(otherTag)) return true;
            }

            return false;
        }
    }
}