

using WolfCurses.Utility;

namespace OregonTrailDotNet.Entity.Location
{
   
    public enum LocationWarning
    {
        
        None = 0,

       
        [Description("Bad Water")] BadWater = 1,

       
        [Description("Low Food")] LowFood = 2,

       
        [Description("Low Grass")] LowGrass = 3,

     
        [Description("Low Water")] LowWater = 4,

       
        [Description("No Food")] NoFood = 5,

      
        [Description("No Water")] NoWater = 6,

        
        Starvation = 7
    }
}