

using System;
using System.Text;
using OregonTrailDotNet.Entity.Vehicle;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace OregonTrailDotNet.Window.Travel.Dialog
{
    
    [ParentWindow(typeof(Travel))]
    public sealed class LocationArrive : InputForm<TravelInfo>
    {
        
        public LocationArrive(IWindow window) : base(window)
        {
        }

        
        
        protected override DialogType DialogType => GameSimulationApp.Instance.Trail.IsFirstLocation
            ? DialogType.Prompt
            : DialogType.YesNo;

        
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();

            // Vehicle is stopped when you are looking around.
            GameSimulationApp.Instance.Vehicle.Status = VehicleStatus.Stopped;
        }

       
        protected override string OnDialogPrompt()
        {
            // Grab instance of game simulation for easy reading.
            var game = GameSimulationApp.Instance;
            var pointReached = new StringBuilder();

            // Build up representation of arrival to new location, depending on location it can change.
            if (game.Trail.IsFirstLocation)
            {
                // First point of interest has slightly different message about time travel.
                pointReached.AppendLine(
                    $"{Environment.NewLine}Going back to {game.Time.CurrentYear}...{Environment.NewLine}");
            }
            else if (game.Trail.LocationIndex < game.Trail.Locations.Count)
            {
                // Build up message about location the player is arriving at.
                pointReached.AppendLine(
                    $"{Environment.NewLine}You are now at the {game.Trail.CurrentLocation.Name}.");
                pointReached.Append("Would you like to look around? Y/N");
            }

            // Wait for input on deciding if we should take a look around.
            return pointReached.ToString();
        }

    
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // First location we will always clear state back to location since it is starting point.
            if (GameSimulationApp.Instance.Trail.IsFirstLocation)
            {
                ClearForm();
                return;
            }

            // Subsequent locations will confirm what the player wants to do, they can stop or keep going on the trail at their own demise.
            switch (reponse)
            {
                case DialogResponse.Custom:
                case DialogResponse.No:
                    var travelMode = ParentWindow as Travel;
                    if (travelMode == null)
                        throw new InvalidCastException(
                            "Unable to cast parent game Windows into travel game Windows when it should be that!");

                    // Call the continue on trail method command inside that game Windows, it will trigger the next action accordingly.
                    travelMode.ContinueOnTrail();
                    break;
                case DialogResponse.Yes:
                    ClearForm();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(reponse), reponse, null);
            }
        }
    }
}