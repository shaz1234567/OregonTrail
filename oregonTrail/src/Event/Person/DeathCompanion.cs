

using System;
using System.Text;
using OregonTrailDotNet.Module.Director;
using OregonTrailDotNet.Window.RandomEvent;

namespace OregonTrailDotNet.Event.Person
{
  
    [DirectorEvent(EventCategory.Person, EventExecution.ManualOnly)]
    public sealed class DeathCompanion : EventProduct
    {
     
        private StringBuilder _passengerDeath;

       
        public override void OnEventCreate()
        {
            base.OnEventCreate();

            _passengerDeath = new StringBuilder();
        }

     
        public override void Execute(RandomEventInfo eventExecutor)
        {
            // Cast the source entity as a passenger from vehicle.
            var sourcePerson = eventExecutor.SourceEntity as Entity.Person.Person;
            if (sourcePerson == null)
                throw new ArgumentNullException(nameof(eventExecutor),
                    "Could not cast source entity as passenger of vehicle.");

            // Check to make sure this player is not the leader (aka the player).
            if (sourcePerson.Leader)
                throw new ArgumentException("Cannot kill this person because it is the player!");

            _passengerDeath.AppendLine($"{sourcePerson.Name} has died.");
        }

      
        protected override string OnRender(RandomEventInfo userData)
        {
            return _passengerDeath.ToString();
        }
    }
}