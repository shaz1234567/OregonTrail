

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using OregonTrailDotNet.Entity;
using OregonTrailDotNet.Event.Prefab;
using OregonTrailDotNet.Module.Director;

namespace OregonTrailDotNet.Event.Wild
{
   
    [DirectorEvent(EventCategory.Wild)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class Thief : ItemDestroyer
    {
       
        protected override string OnPostDestroyItems(IDictionary<Entities, int> destroyedItems)
        {
            // Ammo used to kill the thief is randomly generated.
            GameSimulationApp.Instance.Vehicle.Inventory[Entities.Ammo].ReduceQuantity(
                GameSimulationApp.Instance.Random.Next(1, 5));

            // Change event text depending on if items were destroyed or not.
            return destroyedItems.Count > 0
                ? TryKillPassengers("murdered")
                : "no loss of items.";
        }

     
        protected override string OnPreDestroyItems()
        {
            var theifPrompt = new StringBuilder();
            theifPrompt.Clear();
            theifPrompt.AppendLine("Thief comes in the");
            theifPrompt.Append("night resulting in ");
            return theifPrompt.ToString();
        }
    }
}