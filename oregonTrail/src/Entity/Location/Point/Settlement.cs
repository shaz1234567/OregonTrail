

using OregonTrailDotNet.Entity.Location.Weather;

namespace OregonTrailDotNet.Entity.Location.Point
{
    
    public sealed class Settlement : Location
    {
       
        public Settlement(string name, Climate climateType) : base(name, climateType)
        {
        }

      
        public override bool ChattingAllowed => true;

    
        public override bool ShoppingAllowed => true;
    }
}