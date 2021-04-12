using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CheeseCompositor.Config;
using CheeseCompositor.Config.ModifySteps;

namespace CheeseCompositor.Json
{
    internal class ModifyStepConverter : JsonConverter
    {
        const string TypeKeyName = "type";

        public override bool CanConvert(Type objectType) => typeof(ModifyStep).IsAssignableFrom(objectType);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);

            var modify = GetModifyByType((string)jsonObject[TypeKeyName]);
            serializer.Populate(jsonObject.CreateReader(), modify);

            return modify;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private ModifyStep GetModifyByType(string type) => type switch {
            "color" => new ModifyStepColor(),
            _ => throw new JsonSerializationException($"invalid modify type '{type}' specified"),
        };
    }
}