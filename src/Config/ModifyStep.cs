using Newtonsoft.Json;
using CheeseCompositor.Json;

namespace CheeseCompositor.Config
{
    [JsonConverter(typeof(ModifyStepConverter))]
    internal abstract class ModifyStep
    {

    }
}