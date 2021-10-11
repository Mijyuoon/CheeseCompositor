using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SixLabors.ImageSharp;

namespace CheeseCompositor.Json
{
    internal class ColorConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => typeof(Color).IsAssignableFrom(objectType);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonValue = (string)JToken.Load(reader);

            if (Color.TryParseHex(jsonValue, out var color))
            {
                return color;
            }

            throw new JsonSerializationException($"invalid color value: {jsonValue}");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}