

using System.Diagnostics.CodeAnalysis;
using OregonTrailDotNet.Module.Director;
using OregonTrailDotNet.Window.RandomEvent;

namespace OregonTrailDotNet.Event.Vehicle
{
   
    [DirectorEvent(EventCategory.Vehicle)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class OxenWanderOff : EventProduct
    {
        
        public override void Execute(RandomEventInfo eventExecutor)
        {
            // Cast the source entity as vehicle.
            var vehicle = eventExecutor.SourceEntity as Entity.Vehicle.Vehicle;

            // Reduce the total possible mileage of the vehicle this turn.
            vehicle?.ReduceMileage(17);
        }

        
        protected override string OnRender(RandomEventInfo userData)
        {
            return "ox wanders off---spend time looking for it";
        }
    }
}