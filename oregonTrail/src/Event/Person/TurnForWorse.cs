

using OregonTrailDotNet.Module.Director;
using OregonTrailDotNet.Window.RandomEvent;

namespace OregonTrailDotNet.Event.Person
{
   
    [DirectorEvent(EventCategory.Person, EventExecution.ManualOnly)]
    public sealed class TurnForWorse : EventProduct
    {
       
        public override void Execute(RandomEventInfo eventExecutor)
        {
           
            var person = eventExecutor.SourceEntity as Entity.Person.Person;

          
            person?.Damage(100);
        }

      
        protected override string OnRender(RandomEventInfo userData)
        {
           
            var person = userData.SourceEntity as Entity.Person.Person;

         
            return person == null
                ? "Nobody has taken a turn for the worse."
                : $"{person.Name} has taken a turn for the worse.";
        }
    }
}