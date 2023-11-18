using System;
using Newtonsoft.Json;

namespace _Source.Code._AKFramework.AKTags.Runtime
{
    public class AKTagConverter : JsonConverter<AKTag>
    {
        public override void WriteJson(JsonWriter writer, AKTag value, JsonSerializer serializer)
        {
            writer.WriteValue($"{value._Id}_{value._Name}");
        }

        public override AKTag ReadJson(JsonReader reader, Type objectType, AKTag existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var tag = ((string)reader.Value).Split('_');
            return new AKTag(tag[0], tag[1]);
        }
        
    }
}