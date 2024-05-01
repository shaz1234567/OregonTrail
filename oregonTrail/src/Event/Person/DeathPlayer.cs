

using System;
using System.Text;
using OregonTrailDotNet.Module.Director;
using OregonTrailDotNet.Window.RandomEvent;

namespace OregonTrailDotNet.Event.Person
{
   
    [DirectorEvent(EventCategory.Person, EventExecution.ManualOnly)]
    public sealed class DeathPlayer : EventProduct
    {
      
        private StringBuilder _leaderDeath;

      
        public override void OnEventCreate()
        {
            base.OnEventCreate();

            _leaderDeath = new StringBuilder();
        }

        
        public override void Execute(RandomEventInfo eventExecutor)
        {
            // Cast the source entity as a player.
            var sourcePerson = eventExecutor.SourceEntity as Entity.Person.Person;
            if (sourcePerson == null)
                throw new ArgumentNullException(nameof(eventExecutor), "Could not cast source entity as player.");

            // Check to make sure this player is the leader (aka the player).
            if (!sourcePerson.Leader)
                throw new ArgumentException("Cannot kill this person because it is not the player!");

            _leaderDeath.AppendLine($"{sourcePerson.Name} has died.");
        }

     
        protected override string OnRender(RandomEventInfo userData)
        {
            return _leaderDeath.ToString();
        }
    }
}