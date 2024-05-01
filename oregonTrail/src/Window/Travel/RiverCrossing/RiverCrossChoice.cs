
using WolfCurses.Utility;

namespace OregonTrailDotNet.Window.Travel.RiverCrossing
{
    
    public enum RiverCrossChoice
    {
       
        None = 0,

       
        [Description("attempt to ford the river")] Ford = 1,

        
        [Description("caulk the wagon and float it across")] Float = 2,

      
        
        [Description("take a ferry across")] Ferry = 3,

       
        [Description("hire an Indian to help")] Indian = 4,

      
        [Description("wait to see if conditions improve")] WaitForWeather = 5,

       
        [Description("get more information")] GetMoreInformation = 6
    }
}