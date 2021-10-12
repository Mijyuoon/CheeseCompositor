using Newtonsoft.Json;
using CheeseCompositor.Json;

namespace CheeseCompositor.Config
{
    [JsonConverter(typeof(ModifyStepConverter))]
    internal class ModifyStep
    {
        public const string TypeKeyName = "type";

        [JsonProperty(TypeKeyName)]
        public string Type { get; set; }
    }
}