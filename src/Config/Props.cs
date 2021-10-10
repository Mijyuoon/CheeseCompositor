using Newtonsoft.Json;

namespace CheeseCompositor.Config
{
    internal class Props
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }
        
        [JsonProperty("size", Required = Required.Always)]
        public int Size { get; set; }

        [JsonProperty("inScale")]
        public int InputScale { get; set; } = 1;

        [JsonProperty("outScale")]
        public int OutputScale { get; set; } = 1;
    }
}