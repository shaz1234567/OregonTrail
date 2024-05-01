

using OregonTrailDotNet.Module.Director;
using OregonTrailDotNet.Window.RandomEvent;

namespace OregonTrailDotNet.Event.Prefab
{
   
    public abstract class PersonInfect : EventProduct
    {
        
        public override void Execute(RandomEventInfo eventExecutor)
        {
            // Cast the source entity as person.
            var person = eventExecutor.SourceEntity as Entity.Person.Person;

       
            person?.Infect();
        }

     
        protected override string OnRender(RandomEventInfo userData)
        {
            // Cast the source entity as person.
            var person = userData.SourceEntity as Entity.Person.Person;

            // Skip if the source entity is not a person.
            return person == null ? string.Empty : OnPostInfection(person);
        }

      
        protected abstract string OnPostInfection(Entity.Person.Person person);
    }
}