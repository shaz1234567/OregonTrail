

using System;
using System.Text;
using OregonTrailDotNet.Event.Vehicle;
using OregonTrailDotNet.Window.Travel.Dialog;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace OregonTrailDotNet.Window.Travel.RiverCrossing
{
   
    [ParentWindow(typeof(Travel))]
    public sealed class CrossingResult : InputForm<TravelInfo>
    {
        
        private readonly StringBuilder _crossingResult;

       
        public CrossingResult(IWindow window) : base(window)
        {
            _crossingResult = new StringBuilder();
        }

       
        protected override string OnDialogPrompt()
        {
            // Clear any previous crossing result prompt.
            _crossingResult.Clear();

            // Depending on crossing type we will say different things about the crossing.
            switch (UserData.River.CrossingType)
            {
                case RiverCrossChoice.Ford:
                    if (GameSimulationApp.Instance.Random.NextBool())
                    {
                        // No loss in time, but warning to let the player know it's dangerous.
                        _crossingResult.AppendLine($"{Environment.NewLine}It was a muddy crossing,");
                        _crossingResult.AppendLine("but you did not get");
                        _crossingResult.AppendLine($"stuck.{Environment.NewLine}");
                    }
                    else
                    {
                        // Triggers event for muddy shore that makes player lose a day, forces end of crossing also.
                        FinishCrossing();
                        GameSimulationApp.Instance.EventDirector.TriggerEvent(GameSimulationApp.Instance.Vehicle,
                            typeof(StuckInMud));
                    }

                    break;
                case RiverCrossChoice.Float:
                    if (UserData.River.DisasterHappened)
                    {
                        _crossingResult.AppendLine($"{Environment.NewLine}Your party relieved");
                        _crossingResult.AppendLine("to reach other side after");
                        _crossingResult.AppendLine($"trouble floating across.{Environment.NewLine}");
                    }
                    else
                    {
                        _crossingResult.AppendLine($"{Environment.NewLine}You had no trouble");
                        _crossingResult.AppendLine("floating the wagon");
                        _crossingResult.AppendLine($"across.{Environment.NewLine}");
                    }

                    break;
                case RiverCrossChoice.Ferry:
                    if (UserData.River.DisasterHappened)
                    {
                        _crossingResult.AppendLine($"{Environment.NewLine}The ferry operator");
                        _crossingResult.AppendLine("apologizes for the");
                        _crossingResult.AppendLine($"rough ride.{Environment.NewLine}");
                    }
                    else
                    {
                        _crossingResult.AppendLine($"{Environment.NewLine}The ferry got your party");
                        _crossingResult.AppendLine($"and wagon safely across.{Environment.NewLine}");
                    }

                    break;
                case RiverCrossChoice.Indian:
                    if (UserData.River.DisasterHappened)
                    {
                        _crossingResult.AppendLine($"{Environment.NewLine}The Indian runs away");
                        _crossingResult.AppendLine("as soon as you");
                        _crossingResult.AppendLine($"reach the shore.{Environment.NewLine}");
                    }
                    else
                    {
                        _crossingResult.AppendLine($"{Environment.NewLine}The Indian helped your");
                        _crossingResult.AppendLine($"wagon safely across.{Environment.NewLine}");
                    }

                    break;
                case RiverCrossChoice.None:
                case RiverCrossChoice.WaitForWeather:
                case RiverCrossChoice.GetMoreInformation:
                    throw new InvalidOperationException(
                        $"Invalid river crossing result choice {UserData.River.CrossingType}.");
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Render the crossing result to text user interface.
            return _crossingResult.ToString();
        }

       
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            FinishCrossing();
        }

        
        private void FinishCrossing()
        {
            // Destroy the river data now that we are done with it.
            UserData.DestroyRiver();

            // River crossing takes you a day.
            GameSimulationApp.Instance.TakeTurn(false);

            // Start going there...
            SetForm(typeof(LocationDepart));
        }
    }
}