using Newtonsoft.Json;

namespace CheeseCompositor.Config
{
    internal class Base
    {
        [JsonProperty("img", Required = Required.Always)]
        public string Image { get; set; }

        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("scale")]
        public int Scale { get; set; }
    }
}