

using System.Diagnostics.CodeAnalysis;
using OregonTrailDotNet.Entity;
using OregonTrailDotNet.Module.Director;
using OregonTrailDotNet.Window.RandomEvent;

namespace OregonTrailDotNet.Event.Wild
{
 
    [DirectorEvent(EventCategory.Wild)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class IndiansHelp : EventProduct
    {
        
        public override void Execute(RandomEventInfo eventExecutor)
        {
            // Cast the source entity as vehicle.
            var vehicle = eventExecutor.SourceEntity as Entity.Vehicle.Vehicle;

            // Indians hook you up with free food, what nice guys.
            vehicle?.Inventory[Entities.Food].AddQuantity(14);
        }

        
        protected override string OnRender(RandomEventInfo userData)
        {
            return "helpful Indians show you where to find more food";
        }
    }
}