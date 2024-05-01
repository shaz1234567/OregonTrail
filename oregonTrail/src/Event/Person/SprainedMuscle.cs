

using System.Diagnostics.CodeAnalysis;
using OregonTrailDotNet.Event.Prefab;
using OregonTrailDotNet.Module.Director;

namespace OregonTrailDotNet.Event.Person
{
   
    [DirectorEvent(EventCategory.Person)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class SprainedMuscle : PersonInjure
    {
       
        protected override string OnPostInjury(Entity.Person.Person person)
        {
            return $"{person.Name} has sprained a muscle.";
        }
    }
}