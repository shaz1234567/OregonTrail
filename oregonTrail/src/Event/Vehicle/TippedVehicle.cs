﻿

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using OregonTrailDotNet.Entity;
using OregonTrailDotNet.Event.Prefab;
using OregonTrailDotNet.Module.Director;

namespace OregonTrailDotNet.Event.Vehicle
{
   
    [DirectorEvent(EventCategory.Vehicle)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class TippedVehicle : ItemDestroyer
    {
       
        protected override string OnPostDestroyItems(IDictionary<Entities, int> destroyedItems)
        {
            // Change event text depending on if items were destroyed or not.
            return destroyedItems.Count > 0
                ? TryKillPassengers("crushed")
                : "no loss of items.";
        }

       
        protected override string OnPreDestroyItems()
        {
            var capsizePrompt = new StringBuilder();
            capsizePrompt.Clear();
            capsizePrompt.AppendLine("The wagon tipped over.");
            capsizePrompt.Append("Results in ");
            return capsizePrompt.ToString();
        }
    }
}