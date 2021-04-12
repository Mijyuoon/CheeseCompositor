using System.Collections.Generic;
using Newtonsoft.Json;

namespace CheeseCompositor.Config
{
    internal class Output
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("parts", Required = Required.Always)]
        public IEnumerable<OutputPart> Parts { get; set; }

        [JsonProperty("addX")]
        public int BaseOffsetX { get; set; }

        [JsonProperty("addY")]
        public int BaseOffsetY { get; set; }
    }
}