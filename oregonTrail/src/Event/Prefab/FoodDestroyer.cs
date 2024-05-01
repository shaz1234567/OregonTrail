

using OregonTrailDotNet.Entity;
using OregonTrailDotNet.Module.Director;
using OregonTrailDotNet.Window.RandomEvent;

namespace OregonTrailDotNet.Event.Prefab
{
   
    public abstract class FoodDestroyer : EventProduct
    {
       
        public override void Execute(RandomEventInfo eventExecutor)
        {
            // Cast the source entity as vehicle.
            var vehicle = eventExecutor.SourceEntity as Entity.Vehicle.Vehicle;

            // Skip if the source entity is not a vehicle.
            if (vehicle == null)
                return;

            // Check there is food to even remove.
            if (vehicle.Inventory[Entities.Food].Quantity <= 0)
                return;

            // Check if there is enough food to cut up into four pieces.
            if (vehicle.Inventory[Entities.Food].Quantity < 4)
                return;

            // Determine the amount of food we will destroy up to.
            var spoiledFood = vehicle.Inventory[Entities.Food].Quantity/4;

            // Remove some random amount of food, the minimum being three pieces.
            vehicle.Inventory[Entities.Food].ReduceQuantity(GameSimulationApp.Instance.Random.Next(3, spoiledFood));
        }

      
        protected override string OnRender(RandomEventInfo userData)
        {
            return OnFoodSpoilReason();
        }

        
        protected abstract string OnFoodSpoilReason();
    }
}