using System.Collections.Generic;
using _Source.Code._AKFramework.AKCore.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Source.Code._AKFramework.AKEvents.Runtime
{
    [CreateAssetMenu(menuName = "AKFramework/Events Database")]
    public class AKEventsDatabase : AKDatabase
    {
        public AKEventsGroupContainer[] EventsGroupContainers => eventsGroupContainers;
        
        [SerializeField]
        private AKEventsGroupContainer[] eventsGroupContainers;

        public override string Title => "Events";

#if UNITY_EDITOR

        public override void UpdateAKType()
        {
            foreach (var group in EventsGroupContainers)
            {
                foreach (var sfEvent in group.EventContianers)
                {
                    AKTypeUtilsUpdater.AddID($"{group._Name}/{sfEvent._Name}", sfEvent._Id);
                }
                
                AKTypeUtilsUpdater.AddID($"{group._Name}", group._Id);
            }
        }

        [Button]
        private void UpdateAKEvent()
        {
            UpdateAKType();
            AKTypeUtilsUpdater.UpdateID();
        }
#endif
        
        protected override void Generate(out AKGenerationData[] generationData)
        {
            var productLayers = new Dictionary<int, string>();

            foreach (var layer0 in eventsGroupContainers)
            {
                foreach (var layer1 in layer0.EventContianers)
                {
                    productLayers[layer1._Id] = $"{layer0._Name}/{layer1._Name}";
                }
            }

            generationData = new[]
            {
                new AKGenerationData
                {
                    FileName = "AKEvents",
                    Usings = new[]
                    {
                        "using _Source.Code._AKFramework.AKEvents.Runtime;",
                    },
                    AKType = typeof(AKEvent),
                    Properties = productLayers
                }
            };
        }
    }
}