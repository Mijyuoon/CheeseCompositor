using Newtonsoft.Json;

namespace CheeseCompositor.Config
{
    internal class Anchor
    {
        [JsonProperty("key", Required = Required.Always)]
        public string Key { get; set; }

        [JsonProperty("posX")]
        public int PositionX { get; set; }

        [JsonProperty("posY")]
        public int PositionY { get; set; }
    }
}