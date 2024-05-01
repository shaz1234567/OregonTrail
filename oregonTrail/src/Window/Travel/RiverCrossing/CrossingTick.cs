

using System;
using System.Text;
using OregonTrailDotNet.Entity;
using OregonTrailDotNet.Entity.Vehicle;
using OregonTrailDotNet.Event;
using OregonTrailDotNet.Event.River;
using WolfCurses.Core;
using WolfCurses.Utility;
using WolfCurses.Window;
using WolfCurses.Window.Control;
using WolfCurses.Window.Form;

namespace OregonTrailDotNet.Window.Travel.RiverCrossing
{
   
    [ParentWindow(typeof(Travel))]
    public sealed class CrossingTick : Form<TravelInfo>
    {
       
        private readonly StringBuilder _crossingPrompt;

        
        private bool _finishedCrossingRiver;

      
        private readonly MarqueeBar _marqueeBar;

       
        private int _riverCrossingOfTotalWidth;

       
        private string _swayBarText;

        
       
        public CrossingTick(IWindow window) : base(window)
        {
            // Create the string builder for holding all our text about river crossing as it happens.
            _crossingPrompt = new StringBuilder();

            // Animated sway bar.
            _marqueeBar = new MarqueeBar();
            _swayBarText = _marqueeBar.Step();

            // Sets the crossing percentage to zero.
            _riverCrossingOfTotalWidth = 0;
            _finishedCrossingRiver = false;
        }

        
        public override bool InputFillsBuffer => false;

        
        public override bool AllowInput => _finishedCrossingRiver;

        
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();

            // Grab instance of the game simulation.
            var game = GameSimulationApp.Instance;

            // Park the vehicle if it is not somehow by now.
            game.Vehicle.Status = VehicleStatus.Stopped;

            // Check if ferry operator wants players monies for trip across river.
            if ((UserData.River.FerryCost > 0) &&
                (game.Vehicle.Inventory[Entities.Cash].TotalValue > UserData.River.FerryCost))
            {
                game.Vehicle.Inventory[Entities.Cash].ReduceQuantity((int) UserData.River.FerryCost);

                // Clear out the cost for the ferry since it has been paid for now.
                UserData.River.FerryCost = 0;
            }

            // Check if the Indian guide wants his clothes for the trip that you agreed to.
            if ((UserData.River.IndianCost > 0) &&
                (game.Vehicle.Inventory[Entities.Clothes].Quantity > UserData.River.IndianCost))
            {
                game.Vehicle.Inventory[Entities.Clothes].ReduceQuantity(UserData.River.IndianCost);

                // Clear out the cost for the ferry since it has been paid for now.
                UserData.River.IndianCost = 0;
            }
        }

        
        public override string OnRenderForm()
        {
            // Clears the string buffer for this render pass.
            _crossingPrompt.Clear();

            // Ping-pong progress bar to show that we are moving.
            _crossingPrompt.AppendLine($"{Environment.NewLine}{_swayBarText}");

            // Get instance of game simulation.
            var game = GameSimulationApp.Instance;

            // Shows basic status of vehicle and total river crossing percentage.
            _crossingPrompt.AppendLine(
                "--------------------------------");
            _crossingPrompt.AppendLine(
                $"{game.Trail.CurrentLocation.Name}");
            _crossingPrompt.AppendLine(
                $"{game.Time.Date}");
            _crossingPrompt.AppendLine(
                $"Weather: {game.Trail.CurrentLocation.Weather.ToDescriptionAttribute()}");
            _crossingPrompt.AppendLine(
                $"Health: {game.Vehicle.PassengerHealthStatus.ToDescriptionAttribute()}");
            _crossingPrompt.AppendLine(
                $"Crossing By: {UserData.River.CrossingType}");
            _crossingPrompt.AppendLine(
                $"River width: {UserData.River.RiverWidth:N0} feet");
            _crossingPrompt.AppendLine(
                $"River crossed: {_riverCrossingOfTotalWidth:N0} feet");
            _crossingPrompt.AppendLine(
                "--------------------------------");

            // Wait for user input...
            if (_finishedCrossingRiver)
                _crossingPrompt.AppendLine(InputManager.PRESSENTER);

            return _crossingPrompt.ToString();
        }

       
        public override void OnTick(bool systemTick, bool skipDay)
        {
            base.OnTick(systemTick, skipDay);

            // Skip system ticks.
            if (systemTick)
                return;

            // Stop crossing if we have finished.
            if (_finishedCrossingRiver)
                return;

            // Grab instance of game simulation for easy reading.
            var game = GameSimulationApp.Instance;

            // Advance the progress bar, step it to next phase.
            _swayBarText = _marqueeBar.Step();

            // Increment the amount we have floated over the river.
            _riverCrossingOfTotalWidth += game.Random.Next(1, UserData.River.RiverWidth/4);

            // Check to see if we will finish crossing river before crossing more.
            if (_riverCrossingOfTotalWidth >= UserData.River.RiverWidth)
            {
                _riverCrossingOfTotalWidth = UserData.River.RiverWidth;
                _finishedCrossingRiver = true;
                return;
            }

            // River crossing will allow ticking of people, vehicle, and other important events but others like consuming food are disabled.
            GameSimulationApp.Instance.TakeTurn(true);

            // Attempt to throw a random event related to some failure happening with river crossing.
            switch (UserData.River.CrossingType)
            {
                case RiverCrossChoice.Ford:
                    if ((UserData.River.RiverDepth > 3) && !UserData.River.DisasterHappened &&
                        (_riverCrossingOfTotalWidth >= UserData.River.RiverWidth/2))
                    {
                        UserData.River.DisasterHappened = true;
                        game.EventDirector.TriggerEvent(game.Vehicle, typeof(VehicleWashOut));
                    }
                    else
                    {
                        // Check that we don't flood the user twice, that is just annoying.
                        game.EventDirector.TriggerEventByType(game.Vehicle, EventCategory.RiverCross);
                    }

                    break;
                case RiverCrossChoice.Float:
                    if ((UserData.River.RiverDepth > 5) && !UserData.River.DisasterHappened &&
                        (_riverCrossingOfTotalWidth >= UserData.River.RiverWidth/2) &&
                        game.Random.NextBool())
                    {
                        UserData.River.DisasterHappened = true;
                        game.EventDirector.TriggerEvent(game.Vehicle, typeof(VehicleFloods));
                    }
                    else
                    {
                        // Check that we don't flood the user twice, that is just annoying.
                        game.EventDirector.TriggerEventByType(game.Vehicle, EventCategory.RiverCross);
                    }

                    break;
                case RiverCrossChoice.Ferry:
                case RiverCrossChoice.Indian:
                    game.EventDirector.TriggerEventByType(game.Vehicle, EventCategory.RiverCross);
                    break;
                case RiverCrossChoice.None:
                case RiverCrossChoice.WaitForWeather:
                case RiverCrossChoice.GetMoreInformation:
                    throw new InvalidOperationException(
                        $"Invalid river crossing result choice {UserData.River.CrossingType}.");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

      
        public override void OnInputBufferReturned(string input)
        {
            // Skip if we are still crossing the river.
            if (_riverCrossingOfTotalWidth < UserData.River.RiverWidth)
                return;

            SetForm(typeof(CrossingResult));
        }
    }
}