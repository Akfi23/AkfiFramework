using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace _Source.Code._AKFramework.AKCore.Runtime
{
    public class AKContainer : IAKContainer
    {
        private static readonly IDictionary<Type, AKInjectableTypeInfo> injectableTypes;
        private readonly Dictionary<Type, object> dependencies;

        static AKContainer()
        {
            injectableTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && typeof(IAKInjectable).IsAssignableFrom(t))
                .Select(t => new AKInjectableTypeInfo(t))
                .ToDictionary(t => t.Type, t => t);
        }

        public AKContainer()
        {
            dependencies = new Dictionary<Type, object>();
            Bind<IAKContainer>(this);
        }

        public void Bind(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var type = obj.GetType();

            if (dependencies.ContainsKey(type))
            {
                throw new Exception("Object of this type already exists in the dependency container");
            }

            dependencies[type] = obj;
        }

        public void Bind<T>(object obj)
        {
            if (dependencies.ContainsKey(typeof(T)))
            {
                throw new Exception("Object of this type already exists in the dependency container");
            }

            dependencies[typeof(T)] = obj ?? throw new ArgumentNullException(nameof(obj));
        }


        public T Resolve<T>() where T : class
        {
            return Resolve(typeof(T)) as T;
        }

        public object Resolve(Type type)
        {
            if (dependencies.ContainsKey(type))
            {
                return dependencies[type];
            }

            return dependencies.FirstOrDefault(kvp => type.IsAssignableFrom(kvp.Key)).Value;
        }

        public object[] Bindings => dependencies.Values.ToArray();


        public IEnumerable<T> GetDependencies<T>() where T : class
        {
            var result = new List<T>();

            foreach (var dependency in dependencies)
            {
                var typedDependency = dependency.Value as T;
                if (typedDependency != null)
                {
                    result.Add(typedDependency);
                }
            }

            return result;
        }

        /// <summary>
        /// Inject all dependencies in container
        /// </summary>
        public void Inject()
        {
            foreach (var dependency in dependencies.Values)
            {
                Inject(dependency);
            }
        }

        public void Inject(GameObject gameObject, bool includeInactive = false)
        {
            foreach (var injectable in gameObject.GetComponentsInChildren<IAKInjectable>(includeInactive))
            {
                Inject(injectable);
            }
        }

        public void Inject(object targetObject)
        {
            if (targetObject == null) throw new ArgumentNullException(nameof(targetObject));

            if (!injectableTypes.TryGetValue(targetObject.GetType(), out var injectableType)) return;

            var injectableFields = injectableType.InjectableFields;
            var injectableProperties = injectableType.InjectableProperties;
            var injectableMethods = injectableType.InjectableMethods;

            InjectFields(ref targetObject, ref injectableFields);
            InjectProperties(ref targetObject, ref injectableProperties);
            InjectMethods(ref targetObject, ref injectableMethods);
        }

        private void InjectFields(ref object targetObject, ref IEnumerable<FieldInfo> injectableFields)
        {
            foreach (var field in injectableFields)
            {
                var dependency = Resolve(field.FieldType);
                field.SetValue(targetObject, dependency);
            }
        }

        private void InjectProperties(ref object targetObject, ref IEnumerable<PropertyInfo> injectableProperties)
        {
            foreach (var property in injectableProperties)
            {
                var dependency = Resolve(property.PropertyType);
                property.SetValue(targetObject, dependency);
            }
        }

        private void InjectMethods(ref object targetObject, ref IEnumerable<MethodInfo> injectableMethods)
        {
            foreach (var method in injectableMethods)
            {
                var parameters = method.GetParameters();

                var _dependencies = new object[parameters.Length];

                for (var i = 0; i < method.GetParameters().Length; i++)
                {
                    var parameter = parameters[i];
                    var dependency = Resolve(parameter.ParameterType);
                    _dependencies[i] = dependency;
                }

                method.Invoke(targetObject, _dependencies);
            }
        }
    }
}