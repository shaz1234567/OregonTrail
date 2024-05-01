
using OregonTrailDotNet.Module.Director;
using OregonTrailDotNet.Window.RandomEvent;

namespace OregonTrailDotNet.Event.Person
{

    [DirectorEvent(EventCategory.Person, EventExecution.ManualOnly)]
    public sealed class WellAgain : EventProduct
    {
        
        public override void Execute(RandomEventInfo eventExecutor)
        {
            
            var person = eventExecutor.SourceEntity as Entity.Person.Person;

            // Removes all infections, injuries, and heals the person in full.
            person?.HealEntirely();
        }

        protected override string OnRender(RandomEventInfo userData)
        {
           
            var person = userData.SourceEntity as Entity.Person.Person;

            
            return person == null ? "nobody is well again." : $"{person.Name} is well again.";
        }
    }
}