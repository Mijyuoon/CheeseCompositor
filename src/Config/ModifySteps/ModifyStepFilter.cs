using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CheeseCompositor.Config.ModifySteps
{
    [JsonConverter(typeof(StringEnumConverter))]
    internal enum ModifyFilterMode
    {
        [EnumMember(Value = "gray")]
        Grayscale,
    }

    internal class ModifyStepFilter : ModifyStep
    {
        [JsonProperty("mode", Required = Required.Always)]
        public ModifyFilterMode FilterMode { get; set; }
    }
}