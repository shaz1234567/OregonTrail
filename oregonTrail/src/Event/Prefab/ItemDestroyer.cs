

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OregonTrailDotNet.Entity;
using OregonTrailDotNet.Entity.Person;
using OregonTrailDotNet.Entity.Vehicle;
using OregonTrailDotNet.Module.Director;
using OregonTrailDotNet.Window.RandomEvent;

namespace OregonTrailDotNet.Event.Prefab
{
   
    public abstract class ItemDestroyer : EventProduct
    {
       
        private StringBuilder _eventText;

       
        public override void OnEventCreate()
        {
            base.OnEventCreate();

            // Create the string builder that will hold representation of event action to display for debugging.
            _eventText = new StringBuilder();
        }

      
        internal static string TryKillPassengers(string killVerb)
        {
            // Change event text depending on if items were destroyed or not.
            var postDestroy = new StringBuilder();
            postDestroy.AppendLine($"the loss of:{Environment.NewLine}");

            // Attempts to kill the living passengers of the vehicle.
            var drownedPassengers = GameSimulationApp.Instance.Vehicle.Passengers.TryKill();

            // If the killed passenger list contains any entries we print them out.
            var passengers = drownedPassengers as IList<Entity.Person.Person> ?? drownedPassengers.ToList();
            foreach (var person in passengers)
            {
                // Only proceed if person is actually dead.
                if (person.HealthStatus != HealthStatus.Dead)
                    continue;

                // Last person killed will not add a new line.
                if (passengers.Last() == person)
                    postDestroy.Append($"{person.Name} ({killVerb})");
                else
                    postDestroy.AppendLine($"{person.Name} ({killVerb})");
            }

            // Returns the processed flooding event for rendering.
            return postDestroy.ToString();
        }

    
       
        public override void Execute(RandomEventInfo eventExecutor)
        {
            // Clear out the text from the string builder.
            _eventText.Clear();

            // Show the pre item destruction text if it exists.
            var preDestoyPrompt = OnPreDestroyItems();
            if (!string.IsNullOrEmpty(preDestoyPrompt))
                _eventText.AppendLine(preDestoyPrompt);

            // Destroy some items at random and get a list back of what and how much.
            var destroyedItems = GameSimulationApp.Instance.Vehicle.DestroyRandomItems();

            // Show the post item destruction text if it exists.
            var postDestroyPrompt = OnPostDestroyItems(destroyedItems);
            if (!string.IsNullOrEmpty(postDestroyPrompt))
                _eventText.AppendLine(postDestroyPrompt);

            // Skip if destroyed items count is zero.
            if (!(destroyedItems?.Count > 0))
                return;

            // Loop through all of the destroyed items and add them to string builder.
            foreach (var destroyedItem in destroyedItems)
                if (destroyedItems.Last().Equals(destroyedItem))
                    _eventText.Append($"{destroyedItem.Value:N0} {destroyedItem.Key}");
                else
                    _eventText.AppendLine($"{destroyedItem.Value:N0} {destroyedItem.Key}");
        }

      
        protected abstract string OnPostDestroyItems(IDictionary<Entities, int> destroyedItems);

       
        protected abstract string OnPreDestroyItems();

        
        protected override string OnRender(RandomEventInfo userData)
        {
            return _eventText.ToString();
        }
    }
}