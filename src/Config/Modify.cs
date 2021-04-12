using System.Collections.Generic;
using Newtonsoft.Json;

namespace CheeseCompositor.Config
{
    internal class Modify
    {
        [JsonProperty("key", Required = Required.Always)]
        public string Key { get; set; }

        [JsonProperty("steps", Required = Required.Always)]
        public IEnumerable<ModifyStep> Steps { get; set; }
    }
}