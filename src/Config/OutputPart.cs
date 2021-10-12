using Newtonsoft.Json;

namespace CheeseCompositor.Config
{
    internal class OutputPart
    {
        [JsonProperty("img", Required = Required.Always)]
        public string Image { get; set; }

        [JsonProperty("at")]
        public string Anchor { get; set; }

        [JsonProperty("mod")]
        public string Modify { get; set; }

        [JsonProperty("posX")]
        public int PositionX { get; set; }

        [JsonProperty("posY")]
        public int PositionY { get; set; }

        [JsonProperty("inScale")]
        public int InputScale { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }
    }
}