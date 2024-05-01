

using System.Diagnostics.CodeAnalysis;
using OregonTrailDotNet.Module.Director;
using OregonTrailDotNet.Window.RandomEvent;

namespace OregonTrailDotNet.Event.Vehicle
{
    
    [DirectorEvent(EventCategory.Vehicle)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class BrokenVehiclePart : EventProduct
    {
       
        public override void Execute(RandomEventInfo userData)
        {
            // Cast the source entity as vehicle.
            var vehicle = userData.SourceEntity as Entity.Vehicle.Vehicle;

            // Break some random part on the vehicle.
            vehicle?.BreakRandomPart();
        }

    
        internal override bool OnPostExecute(EventExecutor eventExecutor)
        {
            base.OnPostExecute(eventExecutor);

            // Check to make sure the source entity is a vehicle.
            var vehicle = eventExecutor.UserData.SourceEntity as Entity.Vehicle.Vehicle;

            // Check to make sure we should load the broken vehicle form.
            if (vehicle?.BrokenPart == null)
                return false;

            // Loads form for random event system that deals with broken vehicle parts.
            eventExecutor.SetForm(typeof(VehicleBrokenPrompt));
            return true;
        }

        
        protected override string OnRender(RandomEventInfo userData)
        {
            // Cast the source entity as vehicle.
            var vehicle = userData.SourceEntity as Entity.Vehicle.Vehicle;
            return $"Broken {vehicle?.BrokenPart.Name.ToLowerInvariant()}.";
        }
    }
}