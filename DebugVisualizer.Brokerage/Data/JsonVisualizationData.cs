using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DebugVisualizer.Brokerage.Data
{
    [JsonConverter(typeof(JsonVisualizationDataConverter))]
    public class JsonVisualizationData : VisualizationData
    {
        public static JsonVisualizationData From(string json)
        {
            var value = JsonConvert.DeserializeObject<JObject>(json);
            return From(value);
        }

        public static JsonVisualizationData From(JObject value)
        {
            var clone = ((JObject)value.DeepClone());
            var kind = clone.Property("kind");
            if (kind == null)
            {
                throw new ArgumentException();
            }
            
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, bool>>(kind.Value.ToString());
            if (dictionary.Values.Any(v => v != true))
            {
                throw new ArgumentException();
            }

            var tags = dictionary.Keys.ToArray();
            kind.Remove();
            return new JsonVisualizationData(tags, clone);
        }
        
        public JObject Properties { get; }
        
        public override string[] Tags { get; }
        
        public JsonVisualizationData(string[] tags, JObject properties)
        {
            Properties = properties;
            Tags = tags;
        }
    }
    
    class JsonVisualizationDataConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            var data = (JsonVisualizationData)value!;
            writer.WriteStartObject();
            writer.WritePropertyName("kind");
            serializer.Serialize(writer, data.Kind);

            foreach (var property in data.Properties)
            {
                writer.WritePropertyName(property.Key);
                serializer.Serialize(writer, property.Value);
            }
            writer.WriteEndObject();
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(JsonVisualizationData);
    } 
}