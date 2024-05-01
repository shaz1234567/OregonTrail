

using WolfCurses.Utility;

namespace OregonTrailDotNet.Window.MainMenu
{
    
    public enum MainMenuCommands
    {
       
        [Description("Travel the trail")] TravelTheTrail = 1,

      
        [Description("Learn about the trail")] LearnAboutTheTrail = 2,

       
        [Description("See the Oregon Top Ten")] SeeTheOregonTopTen = 3,

        
        [Description("Choose Management Options")] ChooseManagementOptions = 4,

       
        [Description("End")] CloseSimulation = 5
    }
}