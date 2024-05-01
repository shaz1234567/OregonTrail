

using OregonTrailDotNet.Module.Director;
using OregonTrailDotNet.Window.RandomEvent;

namespace OregonTrailDotNet.Event.Weather
{
   
    [DirectorEvent(EventCategory.Weather)]
    public sealed class HeavyFog : EventProduct
    {
        
        public override void Execute(RandomEventInfo eventExecutor)
        {
            // Cast the source entity as vehicle.
            var vehicle = eventExecutor.SourceEntity as Entity.Vehicle.Vehicle;

            // Reduce the total possible mileage of the vehicle this turn.
            vehicle?.ReduceMileage(vehicle.Mileage - 10 - 5*GameSimulationApp.Instance.Random.Next());
        }

        
        protected override string OnRender(RandomEventInfo userData)
        {
            return "lose your way in heavy fog---time is lost";
        }
    }
}