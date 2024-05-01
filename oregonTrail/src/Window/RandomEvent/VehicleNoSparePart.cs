

using System;
using OregonTrailDotNet.Entity.Vehicle;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace OregonTrailDotNet.Window.RandomEvent
{
   
    [ParentWindow(typeof(RandomEvent))]
    public sealed class VehicleNoSparePart : InputForm<RandomEventInfo>
    {
       
     
        public VehicleNoSparePart(IWindow window) : base(window)
        {
        }

     
        protected override DialogType DialogType => DialogType.Prompt;

       
        public override bool InputFillsBuffer => false;

       
        protected override string OnDialogPrompt()
        {
            return
                $"{Environment.NewLine}Since you don't have a spare {GameSimulationApp.Instance.Vehicle.BrokenPart.Name.ToLowerInvariant()}" +
                $" you must trade for one.{Environment.NewLine}{Environment.NewLine}";
        }

    
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // Check to make sure the source entity is a vehicle.
            var vehicle = UserData.SourceEntity as Vehicle;
            if (vehicle == null)
                return;

            // Ensure the vehicle is broken and unable to continue.
            vehicle.Status = VehicleStatus.Disabled;

            // Removes this form and random event window.
            ParentWindow.RemoveWindowNextTick();
        }
    }
}