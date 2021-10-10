using System.Collections.Generic;
using Newtonsoft.Json;

namespace CheeseCompositor.Config
{
    internal class Root
    {
        [JsonProperty("props", Required = Required.Always)]
        public Props Props { get; set; }

        [JsonProperty("anchor", Required = Required.Always)]
        public IEnumerable<Anchor> Anchors { get; set; }

        [JsonProperty("modify", Required = Required.Always)]
        public IEnumerable<Modify> Modifies { get; set; }

        [JsonProperty("base", Required = Required.Always)]
        public IEnumerable<BasePart> BaseParts { get; set; }

        [JsonProperty("output", Required = Required.Always)]
        public IEnumerable<Output> Outputs { get; set; }
    }
}