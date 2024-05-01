

using System.Diagnostics.CodeAnalysis;
using OregonTrailDotNet.Event.Prefab;
using OregonTrailDotNet.Module.Director;

namespace OregonTrailDotNet.Event.Wild
{
  
    [DirectorEvent(EventCategory.Wild)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class FoodSpoilage : FoodDestroyer
    {
       
        protected override string OnFoodSpoilReason()
        {
            return "Food spoilage.";
        }
    }
}