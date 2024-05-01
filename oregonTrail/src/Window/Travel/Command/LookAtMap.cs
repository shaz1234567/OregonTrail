

using System;
using System.Text;
using OregonTrailDotNet.Entity.Location;
using OregonTrailDotNet.Entity.Location.Point;
using OregonTrailDotNet.Window.Travel.Dialog;
using WolfCurses.Window;
using WolfCurses.Window.Control;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace OregonTrailDotNet.Window.Travel.Command
{
   
    [ParentWindow(typeof(Travel))]
    public sealed class LookAtMap : InputForm<TravelInfo>
    {
    
        public LookAtMap(IWindow window) : base(window)
        {
        }

      
        protected override string OnDialogPrompt()
        {
            // Create visual progress representation of the trail.
            var map = new StringBuilder();
            map.AppendLine($"{Environment.NewLine}Trail progress{Environment.NewLine}");
            map.AppendLine(TextProgress.DrawProgressBar(
                                GameSimulationApp.Instance.Trail.LocationIndex + 1,
                                GameSimulationApp.Instance.Trail.Locations.Count, 32) + Environment.NewLine);

            // Build up a table of location names and if the player has visited them.
            var locationTable = GameSimulationApp.Instance.Trail.Locations.ToStringTable(
                new[] {"Visited", "Location Name"},
                u => u.Status >= LocationStatus.Arrived,
                u => u.Name
            );
            map.AppendLine(locationTable);

            return map.ToString();
        }

        
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // Check if current location is a fork in the road, if so we will return to that form.
            if (GameSimulationApp.Instance.Trail.CurrentLocation is ForkInRoad)
            {
                SetForm(typeof(LocationFork));
                return;
            }

            // Default action is to return to travel menu.
            ClearForm();
        }
    }
}