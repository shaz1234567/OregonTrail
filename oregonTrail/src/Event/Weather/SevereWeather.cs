

using System;
using System.Collections.Generic;
using OregonTrailDotNet.Entity;
using OregonTrailDotNet.Event.Prefab;
using OregonTrailDotNet.Module.Director;

namespace OregonTrailDotNet.Event.Weather
{
  
    [DirectorEvent(EventCategory.Weather, EventExecution.ManualOnly)]
    public sealed class SevereWeather : ItemDestroyer
    {
      
        protected override string OnPostDestroyItems(IDictionary<Entities, int> destroyedItems)
        {
            return destroyedItems.Count > 0
                ? $"time and supplies lost:{Environment.NewLine}"
                : "no items lost.";
        }

      
        protected override string OnPreDestroyItems()
        {
            return "heavy rains---";
        }
    }
}