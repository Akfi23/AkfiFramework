using System.Collections.Generic;

namespace _Source.Code._AKFramework.AKCore.Runtime
{
    public static class AKIDGenerator
    {
        private static readonly HashSet<int> UidMapping = new();
        public static HashSet<int> GetMapping() => UidMapping; 


        public static void Add(int uid)
        {
            UidMapping.Add(uid);
        }

        public static bool Has(int uid) => UidMapping.Contains(uid);

        public static void Remove(int uid)
        {
            if (UidMapping.Contains(uid))
            {
                UidMapping.Remove(uid);
            }
        }

        public static int Generate()
        {
            var uid = 0;
            do
            {
                uid = new System.Random().Next(2147483647);
            } 
            while (UidMapping.Contains(uid));

            return uid;
        }
    }
}