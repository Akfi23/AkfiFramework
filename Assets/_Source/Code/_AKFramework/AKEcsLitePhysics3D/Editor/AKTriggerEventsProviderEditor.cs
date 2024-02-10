using _Source.Code._AKFramework.AKEcsLitePhysics3D.Runtime;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace _Source.Code._AKFramework.AKEcsLitePhysics3D.Editor
{
    public abstract class AKTriggerEventsProviderEditor : OdinEditor
    {
        protected virtual void OnSceneGUI()
        {
            var triggerEventsProvider = target as AKTriggerCastEventsProvider;

            Handles.color = Color.green;
            var currentMatrix = Handles.matrix;

            if (triggerEventsProvider != null)
            {
                Handles.matrix = triggerEventsProvider.transform.localToWorldMatrix;
                Draw(triggerEventsProvider);
            }

            Handles.matrix = currentMatrix;
        }

        protected abstract void Draw(AKTriggerCastEventsProvider target);
    }
}
