

using OregonTrailDotNet.Event.Prefab;
using OregonTrailDotNet.Module.Director;

namespace OregonTrailDotNet.Event.Vehicle
{
    
    [DirectorEvent(EventCategory.Vehicle)]
    // ReSharper disable once UnusedMember.Global
    public sealed class LostTrail : LoseTime
    {
       
        /// <returns>Number of days that should be skipped in the simulation.</returns>
        protected override int DaysToSkip()
        {
            return GameSimulationApp.Instance.Random.Next(1, 3);
        }

        
     
        protected override string OnLostTimeReason()
        {
            return "Lost trail.";
        }
    }
}