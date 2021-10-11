using Newtonsoft.Json;

namespace CheeseCompositor.Config.ModifySteps
{
    internal class ModifyStepOpacity : ModifyStep
    {
        [JsonProperty("val")]
        public float Value { get; set; }
    }
}