
using OregonTrailDotNet.Entity;
using OregonTrailDotNet.Module.Director;
using WolfCurses.Window;

namespace OregonTrailDotNet.Window.RandomEvent
{
  
    public sealed class RandomEventInfo : WindowData
    {
       
        public EventProduct DirectorEvent { get; set; }

        
        public IEntity SourceEntity { get; set; }

     
        public int DaysToSkip { get; internal set; }

      
        public string EventText { get; set; }
    }
}