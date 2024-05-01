

using System.Collections.Generic;
using System.Collections.ObjectModel;
using OregonTrailDotNet.Entity.Location.Weather;

namespace OregonTrailDotNet.Entity.Location.Point
{
  
    public sealed class ForkInRoad : Location
    {
        
        private readonly List<Location> _skipChoices;

       
        public ForkInRoad(string name, Climate climateType, IEnumerable<Location> skipChoices) : base(name, climateType)
        {
            // Offers up a decision when traveling on the trail
            if (skipChoices != null)
                _skipChoices = new List<Location>(skipChoices);
        }

    
        public ReadOnlyCollection<Location> SkipChoices => _skipChoices.AsReadOnly();

      
        public override bool ChattingAllowed => false;

      
        public override bool ShoppingAllowed => false;
    }
}