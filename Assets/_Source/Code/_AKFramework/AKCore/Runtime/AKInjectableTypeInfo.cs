using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace _Source.Code._AKFramework.AKCore.Runtime
{
    internal sealed class AKInjectableTypeInfo
    {
        public Type Type { get; }
        public IEnumerable<FieldInfo> InjectableFields { get; }
        public IEnumerable<PropertyInfo> InjectableProperties { get; }
        public IEnumerable<MethodInfo> InjectableMethods { get; }

        private const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        public AKInjectableTypeInfo(Type type)
        {
            Type = type;
            InjectableFields = GetInjectableFields(type);
            InjectableProperties = GetInjectableProperties(type);
            InjectableMethods = GetInjectableMethods(type);
        }

        public override string ToString()
        {
            return Type.Name;
        }
        
        private IEnumerable<FieldInfo> GetInjectableFields(Type type)
        {
            var fieldInfos = type.GetFields(BINDING_FLAGS).ToList();

            var currentType = type;
            while (currentType.BaseType != typeof(object))
            {
                if (currentType.BaseType == null) break;
                fieldInfos.AddRange(currentType.BaseType.GetFields(BINDING_FLAGS));
                currentType = currentType.BaseType;
            }

            return fieldInfos.Where(f => f.GetCustomAttribute<AKInjectAttribute>() != null);
        }

        private IEnumerable<PropertyInfo> GetInjectableProperties(Type type)
        {
            var propertyInfos = type.GetProperties(BINDING_FLAGS).ToList();

            var currentType = type;
            while (currentType.BaseType != typeof(object))
            {
                if (currentType.BaseType == null) break;
                propertyInfos.AddRange(currentType.BaseType.GetProperties(BINDING_FLAGS));
                currentType = currentType.BaseType;
            }

            return propertyInfos.Where(f => f.GetCustomAttribute<AKInjectAttribute>() != null);
        }

        private IEnumerable<MethodInfo> GetInjectableMethods(Type type)
        {
            var methodInfos = type.GetMethods(BINDING_FLAGS).ToList();

            var currentType = type;
            while (currentType.BaseType != typeof(object))
            {
                if (currentType.BaseType == null) break;
                methodInfos.AddRange(currentType.BaseType.GetMethods(BINDING_FLAGS));
                currentType = currentType.BaseType;
            }

            return methodInfos.Where(f => f.GetCustomAttribute<AKInjectAttribute>() != null);
        }
    }
}