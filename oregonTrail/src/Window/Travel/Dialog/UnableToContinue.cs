
using System;
using System.Text;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace OregonTrailDotNet.Window.Travel.Dialog
{

    [ParentWindow(typeof(Travel))]
    public sealed class UnableToContinue : InputForm<TravelInfo>
    {
        
        public UnableToContinue(IWindow window) : base(window)
        {
        }

      
        public override bool InputFillsBuffer => false;

        
        protected override string OnDialogPrompt()
        {
            var stuckPrompt = new StringBuilder();

            // Check if we are dealing with lack of oxen to pull or broken vehicle parts.
            var brokenVehicle = GameSimulationApp.Instance.Vehicle.BrokenPart != null;

            if (brokenVehicle)
            {
                stuckPrompt.AppendLine($"{Environment.NewLine}You are unable to continue");
                stuckPrompt.AppendLine(
                    $"your journey. You're {GameSimulationApp.Instance.Vehicle.BrokenPart.Name.ToLowerInvariant()}");
                stuckPrompt.AppendLine($"is broken.{Environment.NewLine}");
            }
            else
            {
                stuckPrompt.AppendLine($"{Environment.NewLine}You are unable to continue");
                stuckPrompt.AppendLine("your journey. You have no");
                stuckPrompt.AppendLine($"oxen to pull your wagon.{Environment.NewLine}");
            }

            return stuckPrompt.ToString();
        }

      
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // Returns to travel command menu.
            ClearForm();
        }
    }
}