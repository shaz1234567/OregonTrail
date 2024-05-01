

using WolfCurses.Utility;

namespace OregonTrailDotNet.Entity.Person
{
    
    public enum HealthStatus
    {
       
        Good = 500,

       
        Fair = 400,

       
        Poor = 300,

      
        [Description("Very Poor")] VeryPoor = 200,

      
        Dead = 0
    }
}