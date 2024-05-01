

using WolfCurses.Utility;

namespace OregonTrailDotNet.Window.Travel
{
  
    public enum TravelCommands
    {
      
        [Description("Continue on trail")] ContinueOnTrail = 1,

        [Description("Check supplies")] CheckSupplies = 2,

       
        [Description("Look at map")] LookAtMap = 3,

      
        [Description("Change pace")] ChangePace = 4,

        
        [Description("Change food rations")] ChangeFoodRations = 5,

     
        [Description("Stop to rest")] StopToRest = 6,

       
        [Description("Attempt to trade")] AttemptToTrade = 7,

        [Description("Hunt for food")] HuntForFood = 8,

       
        [Description("Buy supplies")] BuySupplies = 9,

     
        [Description("Talk to people")] TalkToPeople = 10
    }
}