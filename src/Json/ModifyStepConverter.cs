using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CheeseCompositor.Config;
using CheeseCompositor.Config.ModifySteps;

namespace CheeseCompositor.Json
{
    internal class ModifyStepConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => typeof(ModifyStep).IsAssignableFrom(objectType);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);

            var modify = GetModifyByType((string)jsonObject[ModifyStep.TypeKeyName]);
            serializer.Populate(jsonObject.CreateReader(), modify);

            return modify;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private ModifyStep GetModifyByType(string type) => type switch {
            "color" => new ModifyStepColor(),
            "rotate" => new ModifyStepRotate(),
            "opacity" => new ModifyStepOpacity(),
            _ => new ModifyStep(),
        };
    }
}