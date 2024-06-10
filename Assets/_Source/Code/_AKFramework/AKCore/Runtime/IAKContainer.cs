using System;
using UnityEngine;

namespace _Source.Code._AKFramework.AKCore.Runtime
{
    public interface IAKContainer
    {
        void Inject();

        /// <summary>
        /// Inject dependencies on GameObject. Iterate over all components.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="includeInactive"></param>
        void Inject(GameObject gameObject, bool includeInactive = false);

        /// <summary>
        /// Inject dependencies on targetObject
        /// </summary>
        /// <param name="targetObject">Object to add dependencies to</param>
        /// <exception cref="ArgumentNullException"></exception>
        void Inject(object targetObject);

        T Resolve<T>() where T : class;
        object Resolve(Type type);

        object[] Bindings { get; }

        void Bind(object obj);
        void Bind<T>(object obj);
    }
}