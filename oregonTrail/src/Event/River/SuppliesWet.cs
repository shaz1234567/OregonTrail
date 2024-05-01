

using System.Diagnostics.CodeAnalysis;
using OregonTrailDotNet.Event.Prefab;
using OregonTrailDotNet.Module.Director;

namespace OregonTrailDotNet.Event.River
{
 
    [DirectorEvent(EventCategory.RiverCross)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class SuppliesWet : LoseTime
    {
       
        protected override int DaysToSkip()
        {
            return 1;
        }

      
        protected override string OnLostTimeReason()
        {
            return "Your supplies got wet.";
        }
    }
}