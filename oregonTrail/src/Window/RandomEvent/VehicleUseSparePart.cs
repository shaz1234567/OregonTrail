

using System;
using OregonTrailDotNet.Entity.Vehicle;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace OregonTrailDotNet.Window.RandomEvent
{

    [ParentWindow(typeof(RandomEvent))]
    public sealed class VehicleUseSparePart : InputForm<RandomEventInfo>
    {
       
        public VehicleUseSparePart(IWindow window) : base(window)
        {
        }

      
        protected override DialogType DialogType => DialogType.Prompt;

   
        public override bool InputFillsBuffer => false;

        protected override string OnDialogPrompt()
        {
            return $"{Environment.NewLine}You were able to repair the " +
                   $"{GameSimulationApp.Instance.Vehicle.BrokenPart.Name.ToLowerInvariant()} using your spare.{Environment.NewLine}{Environment.NewLine}";
        }

        
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // Check to make sure the source entity is a vehicle.
            var vehicle = UserData.SourceEntity as Vehicle;
            if (vehicle == null)
                return;

            // Ensures the vehicle will be able to continue down the trail.
            vehicle.Status = VehicleStatus.Stopped;

            // Set broken part to nothing.
            vehicle.BrokenPart = null;

            // Removes this form and random event window.
            ParentWindow.RemoveWindowNextTick();
        }
    }
}