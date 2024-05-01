

using System;
using System.Text;
using OregonTrailDotNet.Entity.Location;
using OregonTrailDotNet.Entity.Vehicle;
using OregonTrailDotNet.Window.Travel.Dialog;
using WolfCurses.Window;
using WolfCurses.Window.Control;
using WolfCurses.Window.Form;

namespace OregonTrailDotNet.Window.Travel.Command
{
   
    [ParentWindow(typeof(Travel))]
    public sealed class ContinueOnTrail : Form<TravelInfo>
    {
    
        private StringBuilder _drive;

      
        private MarqueeBar _marqueeBar;

       
        private string _swayBarText;

      
        public ContinueOnTrail(IWindow window) : base(window)
        {
        }

      
        public override bool InputFillsBuffer => false;

        
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();

            // Get instance of game simulation for easy reading.
            var game = GameSimulationApp.Instance;

            // We don't create it in the constructor, will update with ticks.
            _drive = new StringBuilder();

          
            _marqueeBar = new MarqueeBar();
            _swayBarText = _marqueeBar.Step();

            // Vehicle has departed the current location for the next one but you can only depart once.
            if ((game.Trail.DistanceToNextLocation > 0) &&
                (game.Trail.CurrentLocation.Status == LocationStatus.Arrived))
                game.Trail.CurrentLocation.Status = LocationStatus.Departed;
        }

     
        public override string OnRenderForm()
        {
            // Clear whatever was in the string builder last tick.
            _drive.Clear();

            // Ping-pong progress bar to show that we are moving.
            _drive.AppendLine($"{Environment.NewLine}{_swayBarText}");

            // Basic information about simulation.
            _drive.AppendLine(TravelInfo.DriveStatus);

            // Don't add the RETURN KEY text here if we are not actually at a point.
            _drive.Append("Press ENTER to size up the situation");

            // Wait for user input, event, or reaching next location...
            return _drive.ToString();
        }

        public override void OnTick(bool systemTick, bool skipDay)
        {
            base.OnTick(systemTick, skipDay);

            // Only game simulation ticks please.
            if (systemTick)
                return;

            // Get instance of game simulation for easy reading.
            var game = GameSimulationApp.Instance;

            // Checks if the vehicle is stuck or broken, if not it is set to moving state.
            game.Vehicle.CheckStatus();

            // Determine if we should continue down the trail based on current vehicle status.
            switch (game.Vehicle.Status)
            {
                case VehicleStatus.Stopped:
                    return;
                case VehicleStatus.Disabled:
                    // Check if vehicle was able to obtain spare parts for repairs.
                    SetForm(typeof(UnableToContinue));
                    break;
                case VehicleStatus.Moving:
                    // Check if there is a tombstone here, if so we attach question form that asks if we stop or not.
                    _swayBarText = _marqueeBar.Step();
                    if (game.Tombstone.ContainsTombstone(game.Vehicle.Odometer) &&
                        !game.Trail.CurrentLocation.ArrivalFlag)
                    {
                        SetForm(typeof(TombstoneQuestion));
                        return;
                    }

                    // Processes the next turn in the game simulation.
                    game.TakeTurn(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

      
        public override void OnInputBufferReturned(string input)
        {
            // Can only stop the simulation if it is actually running.
            if (!string.IsNullOrEmpty(input))
                return;

            // Stop ticks and close this state.
            if (GameSimulationApp.Instance.Vehicle.Status == VehicleStatus.Moving)
                GameSimulationApp.Instance.Vehicle.Status = VehicleStatus.Stopped;

            // Remove the this form.
            ClearForm();
        }
    }
}