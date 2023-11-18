using _Source.Code._Core.Installers;
using UnityEditor;
using UnityEngine;

namespace _Source.Code._AKFramework.AKCore.Editor
{
    [InitializeOnLoad]
    public class AKSetExecutionOrder
    {
        static AKSetExecutionOrder()
        {
            var scripts = (MonoScript[])Resources.FindObjectsOfTypeAll(typeof(MonoScript));
            foreach (var script in scripts)
            {
                if (!typeof(AKContextRoot).IsAssignableFrom(script.GetClass()) || script.GetClass().IsAbstract) continue;

                if (MonoImporter.GetExecutionOrder(script) >= 0)
                {
                    MonoImporter.SetExecutionOrder(script, -1);
                }
            }
        }
    }
}