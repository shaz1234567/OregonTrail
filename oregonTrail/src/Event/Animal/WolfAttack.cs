

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using OregonTrailDotNet.Entity;
using OregonTrailDotNet.Event.Prefab;
using OregonTrailDotNet.Module.Director;

namespace OregonTrailDotNet.Event.Animal
{
   
    [DirectorEvent(EventCategory.Animal)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class WolfAttack : ItemDestroyer
    {
       
        protected override string OnPostDestroyItems(IDictionary<Entities, int> destroyedItems)
        {
            // Change event text depending on if items were destroyed or not.
            return destroyedItems.Count > 0
                ? TryKillPassengers("mauled")
                : "no loss of items.";
        }

       
        protected override string OnPreDestroyItems()
        {
            var firePrompt = new StringBuilder();
            firePrompt.Clear();
            firePrompt.AppendLine("A pack of wolves attack you in the night!");
            firePrompt.Append("Resulting in ");
            return firePrompt.ToString();
        }
    }
}