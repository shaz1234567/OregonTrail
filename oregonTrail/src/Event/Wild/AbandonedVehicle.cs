

using System;
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
    public sealed class AbandonedVehicle : ItemCreator
    {
        
        protected override string OnPostCreateItems(IDictionary<Entities, int> createdItems)
        {
            return createdItems.Count > 0 ? $"and find:{Environment.NewLine}" : "but it is empty";
        }

       
        protected override string OnPreCreateItems()
        {
            var eventText = new StringBuilder();
            eventText.AppendLine("You find an abandoned wagon,");
            return eventText.ToString();
        }
    }
}