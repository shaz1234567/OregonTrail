

using OregonTrailDotNet.Entity.Location.Weather;

namespace OregonTrailDotNet.Entity.Location.Point
{
    
    public sealed class Landmark : Location
    {
        
        public Landmark(string name, Climate climateType) : base(name, climateType)
        {
        }

     
        
        public override bool ChattingAllowed => false;

      
        public override bool ShoppingAllowed => false;
    }
}