using System.Collections.Generic;
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

        protected override void Generate(out AKGenerationData[] generationData)
        {
            var productLayers = new Dictionary<string, string>();

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