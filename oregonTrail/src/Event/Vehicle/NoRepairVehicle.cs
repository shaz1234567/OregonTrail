
using System.Text;
using OregonTrailDotNet.Module.Director;
using OregonTrailDotNet.Window.RandomEvent;

namespace OregonTrailDotNet.Event.Vehicle
{
  
    [DirectorEvent(EventCategory.Vehicle, EventExecution.ManualOnly)]
    public sealed class NoRepairVehicle : EventProduct
    {
        
        public override void Execute(RandomEventInfo eventExecutor)
        {
            // Nothing to see here, move along...
        }

     
        protected override string OnRender(RandomEventInfo userData)
        {
            var repairPrompt = new StringBuilder();
            repairPrompt.AppendLine("You did not repair the broken ");
            repairPrompt.AppendLine(
                $"{GameSimulationApp.Instance.Vehicle.BrokenPart.Name.ToLowerInvariant()}. You must replace it with a ");
            repairPrompt.Append("spare part.");
            return repairPrompt.ToString();
        }

     
        internal override bool OnPostExecute(EventExecutor eventExecutor)
        {
            // Check to make sure the source entity is a vehicle.
            var vehicle = eventExecutor.UserData.SourceEntity as Entity.Vehicle.Vehicle;

            // Skip if the vehicle is null.
            if (vehicle == null)
                return false;

            // Check to ensure 
            eventExecutor.SetForm(vehicle.TryUseSparePart()
                ? typeof(VehicleUseSparePart)
                : typeof(VehicleNoSparePart));

            // Default response allows event to execute normally.
            return false;
        }
    }
}