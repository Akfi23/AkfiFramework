using System.Numerics;
using UnityEngine;

namespace _Client_.Scripts.Utils.Extensions
{
    public static partial class Extensions
    {
        private static readonly string[] FormatName = {
            "", "K", "M", "t", "q", "Q", "s", "S", "o", "n", "d", "U", "D", "T", 
            "Qt", "Qd", "Sd", "St", "O", "N", "v", "c"
        };

        public static string ToFormat(this BigInteger num)
        {
            var value = (float)num;
            if (value == 0)
                return "0";

            var i = 0;
            while (i + 1 < FormatName.Length && value >= 1000f)
            {
                value /= 1000f;
                i++;
            }

            return $"{value:#.##}{FormatName[i]}";
        }
        
        public static string ToFormat(this int num)
        {
            var value = (float)num;
            if (value == 0)
                return "0";

            var i = 0;
            while (i + 1 < FormatName.Length && value >= 1000f)
            {
                value /= 1000f;
                i++;
            }

            return $"{value:#.##}{FormatName[i]}";
        }

        public static string FormatTime(this float time)
        {
            time = ((int)(time * 10)) / 10f;
            return $"{time}s";
        }

        public static string FormatTimeToNearestUnit(this float time)
        {
            var hour = (int)time / 3600;
            if (hour >= 1) return $"{hour:0}h";
                
            var minutes = (int)time / 60;
            if (minutes >= 1) return $"{minutes:0}m";
            
            return $"{time:0}s";
        }
    }
}