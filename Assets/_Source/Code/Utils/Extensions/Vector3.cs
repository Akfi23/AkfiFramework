using System.Collections.Generic;
using UnityEngine;

namespace _Client_.Scripts.Utils.Extensions
{
    public static partial class Extensions
    {
        public static Quaternion GetLookAtYOnlyRotation(this Transform transform, Vector3 target)
        {
            var lookPos = target - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            rotation.x = transform.localRotation.x;
            rotation.z = transform.localRotation.z;
            return rotation;
        }
        
        public static Vector3 WithX(this Vector3 v, float value) => new Vector3(value, v.y, v.z);
        public static Vector3 WithY(this Vector3 v, float value) => new Vector3(v.x, value, v.z);
        public static Vector3 WithZ(this Vector3 v, float value) => new Vector3(v.x, v.y, value);
        
        public static Vector3 Rotated(this Vector3 vector, Quaternion rotation, Vector3 pivot = default)
        {
            return rotation * (vector - pivot) + pivot;
        }
        
        public static Vector3 Rotated(this Vector3 vector, Vector3 rotation, Vector3 pivot = default) 
        {
            return Rotated(vector, Quaternion.Euler(rotation), pivot);
        }
 
        public static Vector3 Rotated(this Vector3 vector, float x, float y, float z, Vector3 pivot = default)
        {
            return Rotated(vector, Quaternion.Euler(x, y, z), pivot);
        }

        public static Vector3 GetRandomPointOnCircle(this Vector3 pivot, float radius)
        {
            float angle = Random.Range(0f, 2f * Mathf.PI);

            float xOffset = Mathf.Cos(angle) * radius;
            float zOffset = Mathf.Sin(angle) * radius;
            pivot.y = 0;

            return pivot + new Vector3(xOffset, 0f, zOffset);
        }

        // public static Vector3 GetRandomPointOnCircle(this Vector3 pivot, float radius, FastRandom random)
        // {
        //     float angle = random.Range(0f, 2f * Mathf.PI);
        //
        //     float xOffset = Mathf.Cos(angle) * radius;
        //     float zOffset = Mathf.Sin(angle) * radius;
        //     pivot.y = 0;
        //
        //     return pivot + new Vector3(xOffset, 0f, zOffset);
        // }
        
        public static Vector3 GetClosestInCollection(this Vector3 target, Vector3[] list)
        {
            if (list.Length < 1)
                return Vector3.zero;
            
            var index = 0;
            var min = Mathf.Infinity;
            for (int i = 0; i < list.Length; i++)
            {
                var dist = Vector3.Distance(list[i], target);
                if (dist >= min) continue;
                index = i;
                min = dist;
            }
            return list[index];
        }
        
        public static Vector3 GetClosestInCollection(this Vector3 target, List<Vector3> list)
        {
            if (list.Count < 1)
                return Vector3.zero;
            
            var index = 0;
            var min = Mathf.Infinity;
            for (int i = 0; i < list.Count; i++)
            {
                var dist = Vector3.Distance(list[i], target);
                if (dist >= min) continue;
                index = i;
                min = dist;
            }
            return list[index];
        }
        
        public static Vector3 GetClosestInCollectionWithIgnore(this Vector3 target, Vector3[] list, Vector3 ignorePoint)
        {
            var index = 0;
            var min = Mathf.Infinity;
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] == ignorePoint) continue;

                var dist = Vector3.Distance(list[i], target);
                if (dist >= min) continue;
             
                index = i;
                min = dist;
            }

            return list[index];
        }
        
        // public static Vector3 GetRandomVector(FastRandom random)
        // {
        //     var x = random.Range(0, 360);
        //     var y = random.Range(0, 360);
        //     var z = random.Range(0, 360);
        //     return new Vector3(x, y, z);
        // }
        //
        // public static Vector3 GetRandomVectorWithBounds(FastRandom random,float min,float max)
        // {
        //     var x = random.Range(min, max);
        //     var y = random.Range(min, max);
        //     var z = random.Range(min, max);
        //     return new Vector3(x, y, z);
        // }
        
        public static Vector3 RandomVector3()
        {
            Vector3 Result = new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360),
                UnityEngine.Random.Range(0, 360));
            return Result;
        }
        
        public static Vector3 RandomVector3(float XMin, float XMax, float YMin, float YMax, float ZMin, float ZMax)
        {
            Vector3 Result = new Vector3(UnityEngine.Random.Range(XMin, XMax), UnityEngine.Random.Range(YMin, YMax), UnityEngine.Random.Range(ZMin, ZMax));
            return Result;
        }
    }
}