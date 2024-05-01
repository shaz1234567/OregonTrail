

using OregonTrailDotNet.Entity.Location.Weather;
using OregonTrailDotNet.Window.Travel.RiverCrossing;

namespace OregonTrailDotNet.Entity.Location.Point
{
   
    public sealed class RiverCrossing : Location
    {
       
        public RiverCrossing(string name, Climate climateType, RiverOption riverOption = RiverOption.FloatAndFord)
            : base(name, climateType)
        {
            // Set the river option into the location itself.
            RiverCrossOption = riverOption;
        }

     
        public RiverOption RiverCrossOption { get; }

        
        public override bool ChattingAllowed => false;

        public override bool ShoppingAllowed => false;
    }
}