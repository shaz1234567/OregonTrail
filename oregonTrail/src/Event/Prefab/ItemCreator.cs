

using System.Collections.Generic;
using System.Linq;
using System.Text;
using OregonTrailDotNet.Entity;
using OregonTrailDotNet.Module.Director;
using OregonTrailDotNet.Window.RandomEvent;

namespace OregonTrailDotNet.Event.Prefab
{
   
    public abstract class ItemCreator : EventProduct
    {
        
        private StringBuilder _eventText;

       
        public override void OnEventCreate()
        {
            base.OnEventCreate();

          
            _eventText = new StringBuilder();
        }

      
        public override void Execute(RandomEventInfo eventExecutor)
        {
            // Clear out the text from the string builder.
            _eventText.Clear();

            // Add the pre-create message if it exists.
            var preCreatePrompt = OnPreCreateItems();
            if (!string.IsNullOrEmpty(preCreatePrompt))
                _eventText.AppendLine(preCreatePrompt);

            // Create some items at random and get a list back of what and how much.
            var createdItems = GameSimulationApp.Instance.Vehicle.CreateRandomItems();

            // Add the post create message if it exists.
            var postCreatePrompt = OnPostCreateItems(createdItems);
            if (!string.IsNullOrEmpty(postCreatePrompt))
                _eventText.AppendLine(postCreatePrompt);

            // Skip if created items count is zero.
            if (!(createdItems?.Count > 0))
                return;

            // Loop through all of the created items and add them to string builder.
            foreach (var createdItem in createdItems)
                if (createdItems.Last().Equals(createdItem))
                    _eventText.Append($"{createdItem.Value:N0} {createdItem.Key}");
                else
                    _eventText.AppendLine($"{createdItem.Value:N0} {createdItem.Key}");
        }

     
        protected abstract string OnPostCreateItems(IDictionary<Entities, int> createdItems);

       
        protected abstract string OnPreCreateItems();

      
        protected override string OnRender(RandomEventInfo userData)
        {
            return _eventText.ToString();
        }
    }
}