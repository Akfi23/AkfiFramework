using System.Text;

namespace _Client_.Scripts.Utils.Extensions
{
    public static partial class Extensions
    {
        public static int IndexOf(this StringBuilder sb, string value, int startIndex, bool ignoreCase)
        {            
            int index;
            int length = value.Length;
            int maxSearchLength = (sb.Length - length) + 1;

            if (ignoreCase)
            {
                for (int i = startIndex; i < maxSearchLength; ++i)
                {
                    if (char.ToLower(sb[i]) == char.ToLower(value[0]))
                    {
                        index = 1;
                        while ((index < length) && (char.ToLower(sb[i + index]) == char.ToLower(value[index])))
                            ++index;

                        if (index == length)
                            return i;
                    }
                }

                return -1;
            }

            for (int i = startIndex; i < maxSearchLength; ++i)
            {
                if (sb[i] == value[0])
                {
                    index = 1;
                    while ((index < length) && (sb[i + index] == value[index]))
                        ++index;

                    if (index == length)
                        return i;
                }
            }

            return -1;
        }
    }
}