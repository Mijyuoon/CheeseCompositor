using Newtonsoft.Json;

namespace CheeseCompositor.Config
{
    internal class BasePart
    {
        [JsonProperty("img", Required = Required.Always)]
        public string Image { get; set; }

        [JsonProperty("posX")]
        public int PositionX { get; set; }

        [JsonProperty("posY")]
        public int PositionY { get; set; }
        
        [JsonProperty("inScale")]
        public int InputScale { get; set; }
    }
}