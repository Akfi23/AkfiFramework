using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace _Client_.Scripts.Utils.Extensions
{
    public static partial class Extensions
    {
        public static bool ExistValue<T>(this IEnumerable<T> value, [NotNull] Func<T, bool> predicate)
        {
            foreach (var v in value)
            {
                if (predicate.Invoke(v)) return true;
            }

            return false;
        }
        
        public static bool AnyValue<T>(this IEnumerable<T> value, [NotNull]Func<T, bool> predicate)
        {
            foreach (var v in value)
            {
                if (predicate.Invoke(v)) return true;
            }

            return false;
        }
        
        public static bool AllValue<T>(this IEnumerable<T> value, [NotNull]Func<T, bool> predicate)
        {
            foreach (var v in value)
            {
                if (!predicate.Invoke(v)) return false;
            }

            return true;
        }

        public static void ForAllValue<T>(this IEnumerable<T> value, Action<T> action)
        {
            foreach (var v in value)
            {
                action.Invoke(v);
            }
        }

        public static T FirstValue<T>(this IEnumerable<T> value, [NotNull] Func<T, bool> predicate)
        {
            foreach (var v in value)
            {
                if (predicate.Invoke(v)) return v;
            }

            return default;
        }
        
        public static T FirstValue<T>(this IEnumerable<T> value)
        {
            foreach (var v in value)
            {
                return v;
            }

            return default;
        }
        
        public static T LastValue<T>(this IEnumerable<T> value, [NotNull] Func<T, bool> predicate)
        {
            T lastValue = default;
            foreach (var v in value)
            {
                if (predicate.Invoke(v)) lastValue =  v;
            }

            return lastValue;
        }
        
        public static T LastValue<T>(this IEnumerable<T> value)
        {
            T lastValue = default;
            foreach (var v in value)
            {
                lastValue =  v;
            }

            return lastValue;
        }

        public static int LastIndexValue<T>(this List<T> value, [NotNull] Func<T, bool> predicate)
        {
            var index = 0;
            var lastIndex = -1; 
            foreach (var v in value)
            {
                if (predicate.Invoke(v)) lastIndex = index;
                index++;
            }

            return lastIndex;
        }

        public static List<T> FindAllValue<T>(this List<T> value, [NotNull] Func<T, bool> predicate)
        {
            var list = new List<T>();
            foreach (var v in value)
            {
                if(predicate.Invoke(v)) list.Add(v);
            }

            return list;
        }

        public static T FindValue<T>(this List<T> value, [NotNull] Func<T, bool> predicate)
        {
            foreach (var v in value)
            {
                if (predicate.Invoke(v)) return v;
            }

            return default;
        }

        public static int FindIndexValue<T>(this IEnumerable<T> value, [NotNull] Func<T, bool> predicate)
        {
            var index = 0;
            foreach (var v in value)
            {
                if (predicate.Invoke(v)) return index;
                index++;
            }

            return -1;
        }

        public static List<T> FindAllValue<T>(this IEnumerable<T> value, [NotNull] Func<T, bool> predicate)
        {
            var list = new List<T>();
            foreach (var v in value)
            {
                if (predicate.Invoke(v)) list.Add(v);
            }

            return list;
        }

        public static IList<T> RemoveAllValue<T>(this IList<T> value, [NotNull] Func<T, bool> predicate)
        {
            var list = new List<T>();
            foreach (var v in value)
            {
                if(!predicate.Invoke(v)) list.Add(v);
            }

            return list;
        }
        
        public static int CountValue<T>(this IEnumerable<T> list, [NotNull] Func<T, bool> predicate)
        {
            var count = 0;
            foreach (var v in list)
            {
                if (predicate.Invoke(v)) count++;
            }

            return count;
        }

        public static int CountValue<T>(this List<List<T>> list)
        {
            var count = 0;
            foreach (var l in list)
            {
                count += l.Count;
            }

            return count;
        }

        public static int GetIndexWithMaxCount<T>(this List<List<T>> list)
        {
            var maxCount = 0;
            var index = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Count <= maxCount) continue;
                maxCount = list[i].Count;
                index = i;
            }

            return index;
        }
        
        public static int GetIndexWithMinCount<T>(this List<List<T>> list)
        {
            var index = -1;
            var minCount = 99999;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Count >= minCount) continue;
                minCount = list[i].Count;
                index = i;
            }

            return index;
        }

        public static List<T> ToListValue<T>(this IEnumerable<T> array)
        {
            var newList = new List<T>();
            newList.AddRange(array);
            return newList;
        }
    }
}
