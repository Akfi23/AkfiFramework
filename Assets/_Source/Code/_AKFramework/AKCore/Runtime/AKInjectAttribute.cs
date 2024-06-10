using System;
using JetBrains.Annotations;

namespace _Source.Code._AKFramework.AKCore.Runtime
{
    [MeansImplicitUse]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property)]
    public class AKInjectAttribute : Attribute
    {
        
    }
}