
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OregonTrailDotNet.Window.Travel.Rest;
using OregonTrailDotNet.Window.Travel.RiverCrossing.Ferry;
using OregonTrailDotNet.Window.Travel.RiverCrossing.Help;
using OregonTrailDotNet.Window.Travel.RiverCrossing.Indian;
using WolfCurses.Utility;
using WolfCurses.Window;
using WolfCurses.Window.Form;

namespace OregonTrailDotNet.Window.Travel.RiverCrossing
{
   
    [ParentWindow(typeof(Travel))]
    public sealed class RiverCross : Form<TravelInfo>
    {
       
        
        private Dictionary<string, RiverCrossChoice> _choiceMappings;

        
       
        private Dictionary<RiverCrossChoice, Action> _riverActions;

       
        private List<RiverCrossChoice> _riverChoices;

      
        private StringBuilder _riverInfo;

        
        private int _riverOptionsCount;

       
        public RiverCross(IWindow window) : base(window)
        {
        }

       
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();

            // Grab instance of the game simulation.
            var game = GameSimulationApp.Instance;

            // Cast the current location as river crossing.
            var riverLocation = game.Trail.CurrentLocation as Entity.Location.Point.RiverCrossing;
            if (riverLocation == null)
                throw new InvalidCastException(
                    "Unable to cast location as river crossing even though it returns as one!");

            // Re-create the mappings and text information on post create each time.
            _riverOptionsCount = 0;
            _riverChoices =
                new List<RiverCrossChoice>(Enum.GetValues(typeof(RiverCrossChoice)).Cast<RiverCrossChoice>());
            _choiceMappings = new Dictionary<string, RiverCrossChoice>();
            _riverActions = new Dictionary<RiverCrossChoice, Action>();
            _riverInfo = new StringBuilder();

            // Header text for above menu comes from river crossing info object.
            _riverInfo.AppendLine("--------------------------------");
            _riverInfo.AppendLine($"{riverLocation.Name}");
            _riverInfo.AppendLine($"{game.Time.Date}");
            _riverInfo.AppendLine("--------------------------------");
            _riverInfo.AppendLine(
                $"Weather: {riverLocation.Weather.ToDescriptionAttribute()}");
            _riverInfo.AppendLine($"River width: {UserData.River.RiverWidth:N0} feet");
            _riverInfo.AppendLine($"River depth: {UserData.River.RiverDepth:N0} feet");
            _riverInfo.AppendLine("--------------------------------");
            _riverInfo.AppendLine($"You may:{Environment.NewLine}");

            // Loop through all the river choice commands and print them out for the state.
            for (var index = 1; index < _riverChoices.Count; index++)
            {
                // Get the current river choice enumeration value we casted into list.
                var riverChoice = _riverChoices[index];

                // Figure out what kind of river options this location is configured for.
                var allow = false;
                switch (riverChoice)
                {
                    case RiverCrossChoice.Float:
                    case RiverCrossChoice.Ford:
                        // Default float and ford choices that exist on every river.
                        allow = true;
                        break;
                    case RiverCrossChoice.GetMoreInformation:
                    case RiverCrossChoice.WaitForWeather:
                        // Allows player to try and wait out bad weather.
                        allow = true;
                        break;
                    case RiverCrossChoice.None:
                        break;
                    case RiverCrossChoice.Ferry:
                        if (riverLocation.RiverCrossOption == RiverOption.FerryOperator)
                            // Ferry operator costs money.
                            allow = true;
                        break;
                    case RiverCrossChoice.Indian:
                        if (riverLocation.RiverCrossOption == RiverOption.IndianGuide)
                            // Indian wants sets of clothes in exchange for helping float.
                            allow = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                // Only add the river choice if the logic above allows it.
                if (allow)
                    AddRiverChoice(riverChoice);
            }
        }

    
        
        private void AddRiverChoice(RiverCrossChoice riverChoice)
        {
            // Increment the total number of river option mappings we have created.
            _riverOptionsCount++;

            // Add the mapping for text user interface mapping to enumeration value for action invoking below.
            _choiceMappings.Add(_riverOptionsCount.ToString(), riverChoice);

            // Last line should not print new line.
            if (_riverChoices.Last() == riverChoice)
                _riverInfo.Append(_riverOptionsCount + ". " + riverChoice.ToDescriptionAttribute());
            else
                _riverInfo.AppendLine(_riverOptionsCount + ". " + riverChoice.ToDescriptionAttribute());

            // Depending on selection made we will decide on what to do.
            switch (riverChoice)
            {
                case RiverCrossChoice.Ford:
                    _riverActions.Add(riverChoice, delegate
                    {
                        // Driving straight into the river and hoping you don't drown.
                        UserData.River.CrossingType = RiverCrossChoice.Ford;
                        SetForm(typeof(CrossingTick));
                    });
                    break;
                case RiverCrossChoice.Float:
                    _riverActions.Add(riverChoice, delegate
                    {
                        // Floating wagon manually without any help.
                        UserData.River.CrossingType = RiverCrossChoice.Float;
                        SetForm(typeof(CrossingTick));
                    });
                    break;
                case RiverCrossChoice.Ferry:
                    _riverActions.Add(riverChoice, delegate
                    {
                        // Ferry operator charges money and time before player can cross.
                        UserData.River.CrossingType = RiverCrossChoice.Ferry;
                        SetForm(typeof(UseFerryConfirm));
                    });
                    break;
                case RiverCrossChoice.Indian:
                    _riverActions.Add(riverChoice, delegate
                    {
                        // Indian guide helps float wagon across river for sets of clothing.
                        UserData.River.CrossingType = RiverCrossChoice.Indian;
                        SetForm(typeof(IndianGuidePrompt));
                    });
                    break;
                case RiverCrossChoice.WaitForWeather:
                    _riverActions.Add(riverChoice, delegate
                    {
                        // Resting by a river only increments a single day at a time.
                        UserData.DaysToRest = 1;
                        UserData.River.CrossingType = RiverCrossChoice.WaitForWeather;
                        SetForm(typeof(Resting));
                    });
                    break;
                case RiverCrossChoice.GetMoreInformation:
                    _riverActions.Add(riverChoice, delegate
                    {
                        UserData.River.CrossingType = RiverCrossChoice.GetMoreInformation;
                        SetForm(typeof(FordRiverHelp));
                    });
                    break;
                case RiverCrossChoice.None:
                    throw new ArgumentException(
                        "Unable to use river cross choice NONE as a selection since it is only intended for initialization.");
                default:
                    throw new ArgumentOutOfRangeException(nameof(riverChoice),
                        "Unable to cast river cross choice into a valid selection for river crossing.");
            }
        }

        
        public override string OnRenderForm()
        {
            return _riverInfo.ToString();
        }

       
        public override void OnInputBufferReturned(string input)
        {
            // Skip if the input is null or empty.
            if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
                return;

            // Skip if the input does not match any known mapping for river choice.
            if (!_choiceMappings.ContainsKey(input))
                return;

            // Check if the river cross choice exists in the dictionary of choices valid for this river crossing location.
            if (!_riverActions.ContainsKey(_choiceMappings[input]))
                return;

            // Invoke the anonymous delegate method that was created when this form was attached.
            _riverActions[_choiceMappings[input]].Invoke();
        }
    }
}