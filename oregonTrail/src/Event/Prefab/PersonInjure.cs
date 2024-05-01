

using OregonTrailDotNet.Module.Director;
using OregonTrailDotNet.Window.RandomEvent;

namespace OregonTrailDotNet.Event.Prefab
{
   
    public abstract class PersonInjure : EventProduct
    {
       
        public override void Execute(RandomEventInfo eventExecutor)
        {
            // Cast the source entity as person.
            var person = eventExecutor.SourceEntity as Entity.Person.Person;

            // Sets flag on person making them more susceptible to further complications.
            person?.Injure();
        }

        protected override string OnRender(RandomEventInfo userData)
        {
            // Cast the source entity as person.
            var person = userData.SourceEntity as Entity.Person.Person;

            // Skip if the source entity is not a person.
            return person == null ? string.Empty : OnPostInjury(person);
        }

        protected abstract string OnPostInjury(Entity.Person.Person person);
    }
}