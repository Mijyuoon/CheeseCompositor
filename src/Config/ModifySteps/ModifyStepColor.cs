using Newtonsoft.Json;
using SixLabors.ImageSharp;
using CheeseCompositor.Json;

namespace CheeseCompositor.Config.ModifySteps
{
    internal class ModifyStepColor : ModifyStep
    {
        [JsonProperty("src", Required = Required.Always)]
        [JsonConverter(typeof(ColorConverter))]
        public Color Source { get; set; }

        [JsonProperty("dst", Required = Required.Always)]
        [JsonConverter(typeof(ColorConverter))]
        public Color Target { get; set; }

        [JsonProperty("tol")]
        public float Tolerance { get; set; }
    }
}