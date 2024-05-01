

using System.Diagnostics.CodeAnalysis;
using OregonTrailDotNet.Entity;
using OregonTrailDotNet.Module.Director;
using OregonTrailDotNet.Window.RandomEvent;

namespace OregonTrailDotNet.Event.Vehicle
{
    
    [DirectorEvent(EventCategory.Vehicle)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class OxenDied : EventProduct
    {
        
        public override void Execute(RandomEventInfo eventExecutor)
        {
            // Cast the source entity as vehicle.
            var vehicle = eventExecutor.SourceEntity as Entity.Vehicle.Vehicle;

            // Skip if the source entity is not a vehicle.
            if (vehicle == null)
                return;

            // Damages the oxen, could make vehicle stuck.
            vehicle.Inventory[Entities.Animal].ReduceQuantity(1);

            // Reduce the total possible mileage of the vehicle this turn.
            vehicle.ReduceMileage(25);
        }

       
        protected override string OnRender(RandomEventInfo userData)
        {
            return "ox injures leg---you have to put it down";
        }
    }
}