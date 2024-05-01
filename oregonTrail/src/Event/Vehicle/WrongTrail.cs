

using System.Diagnostics.CodeAnalysis;
using OregonTrailDotNet.Event.Prefab;
using OregonTrailDotNet.Module.Director;

namespace OregonTrailDotNet.Event.Vehicle
{
    
    [DirectorEvent(EventCategory.Vehicle)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class WrongTrail : LoseTime
    {
       
        protected override int DaysToSkip()
        {
            return GameSimulationApp.Instance.Random.Next(3, 8);
        }

       
        protected override string OnLostTimeReason()
        {
            return "Wrong trail.";
        }
    }
}