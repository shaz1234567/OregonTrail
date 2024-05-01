
using System;
using System.Text;
using OregonTrailDotNet.Window.Travel.Command;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace OregonTrailDotNet.Window.Travel.Dialog
{
   
    [ParentWindow(typeof(Travel))]
    public sealed class LocationDepart : InputForm<TravelInfo>
    {
        
        public LocationDepart(IWindow window) : base(window)
        {
        }

       
        protected override string OnDialogPrompt()
        {
            // Tell player how far it is to next location before attaching drive state.
            var prompt = new StringBuilder();
            var nextPoint = GameSimulationApp.Instance.Trail.NextLocation;
            prompt.AppendLine(
                $"{Environment.NewLine}From {GameSimulationApp.Instance.Trail.CurrentLocation.Name} it is {GameSimulationApp.Instance.Trail.DistanceToNextLocation}");
            prompt.AppendLine($"miles to {nextPoint.Name}{Environment.NewLine}");
            return prompt.ToString();
        }

        
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            SetForm(typeof(ContinueOnTrail));
        }
    }
}