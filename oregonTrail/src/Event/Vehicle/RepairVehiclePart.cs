

using System;
using OregonTrailDotNet.Entity.Vehicle;
using OregonTrailDotNet.Module.Director;
using OregonTrailDotNet.Window.RandomEvent;

namespace OregonTrailDotNet.Event.Vehicle
{
    
    [DirectorEvent(EventCategory.Vehicle, EventExecution.ManualOnly)]
    public sealed class RepairVehiclePart : EventProduct
    {
       
        public override void Execute(RandomEventInfo userData)
        {
            // Nothing to see here, move along...
        }

       
        internal override bool OnPostExecute(EventExecutor eventExecutor)
        {
            // Check to make sure the source entity is a vehicle.
            var vehicle = eventExecutor.UserData.SourceEntity as Entity.Vehicle.Vehicle;
            if (vehicle == null)
                return true;

            // Ensures the vehicle will be able to continue down the trail.
            vehicle.Status = VehicleStatus.Stopped;
            return false;
        }

 
        protected override string OnRender(RandomEventInfo userData)
        {
            return $"{Environment.NewLine}You were able to repair the " +
                   $"{GameSimulationApp.Instance.Vehicle.BrokenPart.Name.ToLowerInvariant()}.{Environment.NewLine}";
        }
    }
}