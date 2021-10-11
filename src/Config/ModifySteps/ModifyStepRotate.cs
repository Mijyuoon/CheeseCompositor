using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CheeseCompositor.Config.ModifySteps
{
    [JsonConverter(typeof(StringEnumConverter))]
    internal enum ModifyRotateMode
    {
        [EnumMember(Value = "left")]
        RotateLeft,
        
        [EnumMember(Value = "right")]
        RotateRight,
        
        [EnumMember(Value = "180")]
        Rotate180,
        
        [EnumMember(Value = "hflip")]
        FlipHorizontal,
        
        [EnumMember(Value = "vflip")]
        FlipVertical,
    }
    
    internal class ModifyStepRotate : ModifyStep
    {
        [JsonProperty("mode")]
        public ModifyRotateMode RotateMode { get; set; }
    }
}