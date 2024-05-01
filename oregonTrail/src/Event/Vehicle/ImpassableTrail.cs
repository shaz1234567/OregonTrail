

using System.Diagnostics.CodeAnalysis;
using OregonTrailDotNet.Event.Prefab;
using OregonTrailDotNet.Module.Director;

namespace OregonTrailDotNet.Event.Vehicle
{
   
    [DirectorEvent(EventCategory.Vehicle)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class ImpassableTrail : LoseTime
    {
       
        protected override int DaysToSkip()
        {
            return GameSimulationApp.Instance.Random.Next(2, 6);
        }

        protected override string OnLostTimeReason()
        {
            return "Impassable trail--lose time going around.";
        }
    }
}