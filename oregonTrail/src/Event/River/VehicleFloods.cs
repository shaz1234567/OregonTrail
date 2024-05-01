

using System.Collections.Generic;
using System.Text;
using OregonTrailDotNet.Entity;
using OregonTrailDotNet.Event.Prefab;
using OregonTrailDotNet.Module.Director;
using OregonTrailDotNet.Window.RandomEvent;

namespace OregonTrailDotNet.Event.River
{
   
    [DirectorEvent(EventCategory.RiverCross, EventExecution.ManualOnly)]
    public sealed class VehicleFloods : ItemDestroyer
    {
       
        protected override string OnPostDestroyItems(IDictionary<Entities, int> destroyedItems)
        {
            return destroyedItems.Count > 0
                ? TryKillPassengers("drowned")
                : "no loss of items.";
        }

       
        public override void Execute(RandomEventInfo eventExecutor)
        {
            base.Execute(eventExecutor);

            // Cast the source entity as vehicle.
            var vehicle = eventExecutor.SourceEntity as Entity.Vehicle.Vehicle;

            // Reduce the total possible mileage of the vehicle this turn.
            vehicle?.ReduceMileage(20 - 20*GameSimulationApp.Instance.Random.Next());
        }

        protected override string OnPreDestroyItems()
        {
            var floodPrompt = new StringBuilder();
            floodPrompt.Clear();
            floodPrompt.AppendLine("Vehicle floods");
            floodPrompt.AppendLine("while crossing the");
            floodPrompt.Append("river results in");
            return floodPrompt.ToString();
        }
    }
}