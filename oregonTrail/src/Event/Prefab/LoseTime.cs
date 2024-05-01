
using OregonTrailDotNet.Module.Director;
using OregonTrailDotNet.Window.RandomEvent;

namespace OregonTrailDotNet.Event.Prefab
{
   
    public abstract class LoseTime : EventProduct
    {
       
        public override void Execute(RandomEventInfo eventExecutor)
        {
            // Add to the days to skip since multiple events in a chain could keep adding to the total.
            eventExecutor.DaysToSkip += DaysToSkip();
        }

      
        internal override bool OnPostExecute(EventExecutor eventExecutor)
        {
            base.OnPostExecute(eventExecutor);

            // Check what we should do with the random event form now that the user is done with this part of it.
            if (eventExecutor.UserData.DaysToSkip > 0)
                return false;

            // Attaches a new form that will skip over the required number of days we have detected.
            eventExecutor.SetForm(typeof(EventSkipDay));
            return true;
        }

    
        protected abstract int DaysToSkip();

        
        protected override string OnRender(RandomEventInfo userData)
        {
            return OnLostTimeReason();
        }

        
        protected abstract string OnLostTimeReason();
    }
}