using System;

namespace _Client_.Scripts.Utils.Extensions
{
    public static partial class Extensions
    {
        public static T[] RemoveAt<T>(this T[] array, int index)
        {
            for (int i = index + 1; i < array.Length; i++)
            {
                array[i - 1] = array[i];
            }
            
            Array.Resize(ref array, array.Length - 1);

            return array;
        }

        public static T[] Add<T>(this T[] array, T element)
        {
            Array.Resize(ref array, array.Length + 1);
            array[^1] = element;

            return array;
        }
    }
}