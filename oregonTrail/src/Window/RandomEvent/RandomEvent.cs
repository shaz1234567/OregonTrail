

using OregonTrailDotNet.Entity;
using OregonTrailDotNet.Module.Director;
using WolfCurses;
using WolfCurses.Window;

namespace OregonTrailDotNet.Window.RandomEvent
{
    
    public sealed class RandomEvent : Window<RandomEventCommands, RandomEventInfo>
    {
      
        public RandomEvent(SimulationApp simUnit) : base(simUnit)
        {
        }

     
        public override void OnWindowPostCreate()
        {
            // Event director has event to know when events are triggered.
            GameSimulationApp.Instance.EventDirector.OnEventTriggered += Director_OnEventTriggered;
        }

       
        public override void OnWindowActivate()
        {
            
        }

       
        public override void OnWindowAdded()
        {
            // Nothing to see here, move along...
        }

      
        private void Director_OnEventTriggered(IEntity simEntity, EventProduct directorEvent)
        {
            // Attached the random event state when we intercept an event it would like us to trigger.
            UserData.DirectorEvent = directorEvent;
            UserData.SourceEntity = simEntity;
            SetForm(typeof(EventExecutor));
        }

        /// <summary>Fired when this game Windows is removed from the list of available and ticked modes in the simulation.</summary>
        protected override void OnModeRemoved()
        {
            base.OnModeRemoved();

            // Event director has event for when he triggers events.
            if (GameSimulationApp.Instance.EventDirector != null)
                GameSimulationApp.Instance.EventDirector.OnEventTriggered -= Director_OnEventTriggered;
        }
    }
}