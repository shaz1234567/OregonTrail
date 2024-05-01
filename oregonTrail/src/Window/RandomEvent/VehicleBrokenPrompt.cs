


using System;
using System.Text;
using OregonTrailDotNet.Event.Vehicle;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace OregonTrailDotNet.Window.RandomEvent
{
   
    [ParentWindow(typeof(RandomEvent))]
    public sealed class VehicleBrokenPrompt : InputForm<RandomEventInfo>
    {
       
        public VehicleBrokenPrompt(IWindow window) : base(window)
        {
        }

     
        protected override DialogType DialogType => DialogType.YesNo;

      
        protected override string OnDialogPrompt()
        {
            var brokenPrompt = new StringBuilder();
            brokenPrompt.AppendLine(
                $"{Environment.NewLine}Broken {GameSimulationApp.Instance.Vehicle.BrokenPart.Name.ToLowerInvariant()}. Would you");
            brokenPrompt.Append("like to try and repair it? Y/N");
            return brokenPrompt.ToString();
        }

     
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // Get game instance to improve readability.
            var game = GameSimulationApp.Instance;

            switch (reponse)
            {
                case DialogResponse.No:
                case DialogResponse.Custom:
                    // Player has chosen to not attempt a repair and opted instead for replacement with spare part.
                    game.EventDirector.TriggerEvent(game.Vehicle, typeof(NoRepairVehicle));
                    break;
                case DialogResponse.Yes:
                    // Depending on dice roll player might be able to fix their broken vehicle part.
                    game.EventDirector.TriggerEvent(game.Vehicle,
                        game.Random.NextBool() ? typeof(RepairVehiclePart) : typeof(NoRepairVehicle));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(reponse), reponse, null);
            }
        }
    }
}