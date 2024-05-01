

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
    public sealed class BanditsAttack : ItemDestroyer
    {
        
        protected override string OnPostDestroyItems(IDictionary<Entities, int> destroyedItems)
        {
            // Ammo used to kill the bandits is randomly generated.
            GameSimulationApp.Instance.Vehicle.Inventory[Entities.Ammo].ReduceQuantity(
                GameSimulationApp.Instance.Random.Next(3, 15));

            // Change event text depending on if items were destroyed or not.
            return destroyedItems.Count > 0
                ? TryKillPassengers("murdered")
                : "no loss of items. You drove them off!";
        }

      
        protected override string OnPreDestroyItems()
        {
            var firePrompt = new StringBuilder();
            firePrompt.Clear();
            firePrompt.AppendLine("Bandits attack!");
            firePrompt.Append("Resulting in ");
            return firePrompt.ToString();
        }
    }
}