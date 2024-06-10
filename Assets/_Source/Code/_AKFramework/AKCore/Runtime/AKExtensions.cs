using System.Collections.Generic;
using _Source.Code._Core.Installers;
using UnityEngine;
using static UnityEngine.Networking.UnityWebRequest;

namespace _Source.Code._AKFramework.AKCore.Runtime
{
    public static partial class AKExtensions
    {
        // public static T SFInstantiate<T>(this T prefab) where T : Object, IAKInjectable
        // {
        //     if (prefab == null) return null;
        //
        //     var obj = Object.Instantiate(prefab);
        //     AKContextRoot.Container.Inject(obj);
        //     return obj;
        // }
        //
        // public static T AKInstantiate<T>(this T prefab, Vector3 position) where T : Object, IAKInjectable
        // {
        //     if (prefab == null) return null;
        //     var obj = Object.Instantiate(prefab, position, Quaternion.identity);
        //     AKContextRoot.Container.Inject(obj);
        //     return obj;
        // }
        //
        // public static T AKInstantiate<T>(this T prefab, Vector3 position, Quaternion rotation)
        //     where T : Object, IAKInjectable
        // {
        //     if (prefab == null) return null;
        //     var obj = Object.Instantiate(prefab, position, rotation);
        //     AKContextRoot.Container.Inject(obj);
        //     return obj;
        // }
        //
        // public static GameObject AKInstantiate(this GameObject prefab, Vector3 position, Quaternion rotation)
        // {
        //     if (prefab == null) return null;
        //     var obj = Object.Instantiate(prefab, position, rotation);
        //     AKContextRoot.Container.Inject(obj);
        //     return obj;
        // }
        //
        // public static T AKInstantiate<T>(this GameObject prefab, Vector3 position, Quaternion rotation)
        //     where T : Component
        // {
        //     if (prefab == null) return null;
        //     var obj = Object.Instantiate(prefab, position, rotation);
        //     AKContextRoot.Container.Inject(obj);
        //     return obj.GetComponent<T>();
        // }
        //
        // public static T AKInstantiate<T>(this T prefab, Vector3 position, Quaternion rotation, Transform parent)
        //     where T : Object, IAKInjectable
        // {
        //     if (prefab == null) return null;
        //     var obj = Object.Instantiate(prefab, position, rotation, parent);
        //     AKContextRoot.Container.Inject(obj);
        //     return obj;
        // }
        //
        // public static T AKInstantiate<T>(this T prefab, Transform parent, bool instantiateInWorldSpace)
        //     where T : Object, IAKInjectable
        // {
        //     if (prefab == null) return null;
        //     var obj = Object.Instantiate(prefab, parent, instantiateInWorldSpace);
        //     AKContextRoot.Container.Inject(obj);
        //     return obj;
        // }

        public static float DistanceXZ(this Vector3 a, Vector3 b)
        {
            a.y = b.y = 0f;
            return Vector3.Distance(a, b);
        }

        public static Vector3 GetCenterPoint(this Vector3[] positions)
        {
            var totalX = 0f;
            var totalY = 0f;
            var totalZ = 0f;
            foreach(var position in positions)
            {
                totalX += position.x;
                totalY += position.y;
                totalZ += position.z;
            }

            return new Vector3(totalX / positions.Length, totalY / positions.Length, totalZ / positions.Length);
        }
        
        public static Vector3 GetCenterPoint(this Transform[] transforms)
        {
            var totalX = 0f;
            var totalY = 0f;
            var totalZ = 0f;
            foreach(var transform in transforms)
            {
                var position = transform.position;
                totalX += position.x;
                totalY += position.y;
                totalZ += position.z;
            }

            return new Vector3(totalX / transforms.Length, totalY / transforms.Length, totalZ / transforms.Length);
        }

        public static void LookAtY(this Transform transform, Vector3 target)
        {
            var lookPos = target - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = rotation;
        }

        public static string MyEscapeURL(this string url) => EscapeURL(url).Replace("+", "%20");
        
        public static string FormatNumber(this int num)
        {
            switch (num)
            {
                case >= 100000000:
                    return (num / 1000000D).ToString("0.#M");
                case >= 1000000:
                    return (num / 1000000D).ToString("0.##M");
                case >= 100000:
                    return (num / 1000D).ToString("0.#k");
                case >= 10000:
                    return (num / 1000D).ToString("0.##k");
                default:
                    return num.ToString("#,0");
            }
        }

        public static string FormatTime(this float time, bool hourInclude = false)
        {
            if (hourInclude)
            {
                var hour = (int)time / 3600;
                var minutes = (int)time / 60 - 60 * hour;
                var seconds = (int)time - (3600 * hour + 60 * minutes);

                return $"{hour:00}h {minutes:00}m {seconds:00}s";
            }
            else
            {
                var minutes = (int)time / 60;
                var seconds = (int)time - 60 * minutes;
                return minutes > 0
                    ? $"{minutes:00}m {seconds:00}s"
                    : (seconds > 10 ? $"{seconds:00}s" : $"{seconds:0}s");
            }
        }

        public static float GetRandomFloatWithStep(int min, int max, float step = 0.5f)
        {
            return Random.Range((int) (min / step), (int) (max / step)) * step;
        }

        public static List<T> Shuffle<T>(this List<T> list)  
        {  
            var count = list.Count;
            while (count > 1)
            {
                count--;
                var k = Random.Range(0, count);
                (list[k], list[count]) = (list[count], list[k]);
            }

            return list;
        }
    }
}