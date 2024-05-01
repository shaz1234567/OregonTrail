
using System;
using System.Text;
using OregonTrailDotNet.Entity.Location;
using OregonTrailDotNet.Entity.Location.Point;
using OregonTrailDotNet.Entity.Vehicle;
using OregonTrailDotNet.Window.Travel.Dialog;
using OregonTrailDotNet.Window.Travel.RiverCrossing;
using WolfCurses.Window;
using WolfCurses.Window.Form;

namespace OregonTrailDotNet.Window.Travel.Rest
{
    
    [ParentWindow(typeof(Travel))]
    public sealed class Resting : Form<TravelInfo>
    {
       
        private int _daysRested;

       
       
        private readonly StringBuilder _restMessage;

   
        public Resting(IWindow window) : base(window)
        {
            _restMessage = new StringBuilder();
        }

        
        public override bool InputFillsBuffer => false;

        
        public override void OnTick(bool systemTick, bool skipDay)
        {
            base.OnTick(systemTick, skipDay);

            // Skip system ticks.
            if (systemTick)
                return;

            // Not ticking when days to rest is zero.
            if (UserData.DaysToRest <= 0)
                return;

            // Check if we are at a river crossing and need to subtract from ferry days also.
            if ((UserData.River != null) &&
                (UserData.River.FerryDelayInDays > 0) &&
                GameSimulationApp.Instance.Trail.CurrentLocation is Entity.Location.Point.RiverCrossing)
                UserData.River.FerryDelayInDays--;

            // Decrease number of days needed to rest, increment number of days rested.
            UserData.DaysToRest--;

            // Increment the number of days reseted.
            _daysRested++;

            // Simulate the days to rest in time and event system, this will trigger random event game Windows if required.
            GameSimulationApp.Instance.TakeTurn(false);
        }

       
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();

            // Only change the vehicle status to stopped if it is moving, it could just be stuck.
            if (GameSimulationApp.Instance.Vehicle.Status == VehicleStatus.Moving)
                GameSimulationApp.Instance.Vehicle.Status = VehicleStatus.Stopped;
        }

        
        public override string OnRenderForm()
        {
            // String that holds message about resting, it can change depending on location.
            _restMessage.Clear();

            // Change up resting prompt depending on location category to give it some context.
            if (GameSimulationApp.Instance.Trail.CurrentLocation is ForkInRoad)
            {
                if (_daysRested > 1)
                    _restMessage.AppendLine($"{Environment.NewLine}You camp near the river for {_daysRested} days.");
                else if (_daysRested == 1)
                    _restMessage.AppendLine($"{Environment.NewLine}You camp near the river for a day.");
                else if (_daysRested <= 0)
                    _restMessage.AppendLine($"{Environment.NewLine}Preparing to camp near the river...");
            }
            else
            {
                if (_daysRested > 1)
                    _restMessage.AppendLine($"{Environment.NewLine}You rest for {_daysRested} days");
                else if (_daysRested == 1)
                    _restMessage.AppendLine($"{Environment.NewLine}You rest for a day.");
                else if (_daysRested <= 0)
                    _restMessage.AppendLine($"{Environment.NewLine}Preparing to rest...");
            }

            // Allow the user to stop resting, this will break the cycle and reset days to rest to zero.
            if (_daysRested > 0)
                _restMessage.AppendLine($"{Environment.NewLine}Press ENTER to stop resting.{Environment.NewLine}");

            // Prints out the message about resting for however long this cycle was.
            return _restMessage.ToString();
        }

        
        public override void OnInputBufferReturned(string input)
        {
            // Figure out what to do with response.
            if (_daysRested > 0)
                StopResting();
        }

       
        private void StopResting()
        {
            // Determine if we should bounce back to travel menu or some special Windows.
            if (UserData.River == null)
            {
                ClearForm();
                return;
            }

            // Check if we have already departed from current location, so we just return to travel menu.
            if (GameSimulationApp.Instance.Trail.CurrentLocation.ArrivalFlag &&
                (GameSimulationApp.Instance.Trail.CurrentLocation.Status == LocationStatus.Departed))
            {
                ClearForm();
                return;
            }

            // Locations can return to a special state if required based on the category of the location.
            if (GameSimulationApp.Instance.Trail.CurrentLocation is Landmark ||
                GameSimulationApp.Instance.Trail.CurrentLocation is Settlement)
            {
                ClearForm();
            }
            else if (GameSimulationApp.Instance.Trail.CurrentLocation is Entity.Location.Point.RiverCrossing)
            {
                UserData.DaysToRest = 0;

                // Player might be crossing a river, so we check if they made a decision and are waiting for ferry operator.
                if ((UserData.River != null) &&
                    (UserData.River.CrossingType == RiverCrossChoice.Ferry) &&
                    (UserData.River.FerryDelayInDays <= 0) &&
                    (UserData.River.FerryCost >= 0))
                    SetForm(typeof(CrossingTick));
                else
                    SetForm(typeof(RiverCross));
            }
            else if (GameSimulationApp.Instance.Trail.CurrentLocation is ForkInRoad)
            {
                SetForm(typeof(LocationFork));
            }
        }
    }
}