using System;
using UnityEngine;

namespace _Source.Code._AKFramework.AKTags.Runtime
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AKTagsGroupAttribute : PropertyAttribute
    {
        public readonly string[] groups;
        
        public AKTagsGroupAttribute(params string[] groupName)
        {
            groups = groupName;
        }
    }
}