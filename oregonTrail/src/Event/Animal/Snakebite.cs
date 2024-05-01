

using System.Diagnostics.CodeAnalysis;
using OregonTrailDotNet.Entity;
using OregonTrailDotNet.Module.Director;
using OregonTrailDotNet.Window.RandomEvent;

namespace OregonTrailDotNet.Event.Animal
{
    
    [DirectorEvent(EventCategory.Animal)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class Snakebite : EventProduct
    {
        
        public override void Execute(RandomEventInfo eventExecutor)
        {
         
            var person = eventExecutor.SourceEntity as Entity.Person.Person;

            // Skip if the source entity is not a person.
            if (person == null)
                return;

            // Ammo used to kill the snake.
            GameSimulationApp.Instance.Vehicle.Inventory[Entities.Ammo].ReduceQuantity(10);

            // Damage the person that was bit by the snake, it might be a little or a huge poisonousness bite.
            if (GameSimulationApp.Instance.Random.NextBool())
            {
                person.Infect();
                person.Damage(256);
            }
            else
            {
                person.Damage(5);
            }
        }

   
        protected override string OnRender(RandomEventInfo userData)
        {
            return "You killed a poisonous snake, after it bit you.";
        }
    }
}