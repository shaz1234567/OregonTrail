

using System.Collections.Generic;
using System.Text;
using OregonTrailDotNet.Entity;
using OregonTrailDotNet.Event.Prefab;
using OregonTrailDotNet.Module.Director;
using OregonTrailDotNet.Window.RandomEvent;

namespace OregonTrailDotNet.Event.Weather
{
    
    [DirectorEvent(EventCategory.Weather, EventExecution.ManualOnly)]
    public sealed class HailStorm : ItemDestroyer
    {
        
        protected override string OnPostDestroyItems(IDictionary<Entities, int> destroyedItems)
        {
            // Grab an instance of the game simulation.
            var game = GameSimulationApp.Instance;

            // Check if there are enough clothes to keep people warm, need two sets of clothes for every person.
            return (game.Vehicle.Inventory[Entities.Clothes].Quantity >= game.Vehicle.PassengerLivingCount*2) &&
                   (destroyedItems.Count < 0)
                ? "no loss of items."
                : TryKillPassengers("frozen");
        }

  
       
        public override void Execute(RandomEventInfo eventExecutor)
        {
            base.Execute(eventExecutor);

            // Cast the source entity as vehicle.
            var vehicle = eventExecutor.SourceEntity as Entity.Vehicle.Vehicle;

            // Reduce the total possible mileage of the vehicle this turn.
            vehicle?.ReduceMileage(vehicle.Mileage - 5 - GameSimulationApp.Instance.Random.Next()*10);
        }

       
        protected override string OnPreDestroyItems()
        {
            var floodPrompt = new StringBuilder();
            floodPrompt.Clear();
            floodPrompt.AppendLine("Severe hail storm");
            floodPrompt.Append("results in");
            return floodPrompt.ToString();
        }
    }
}