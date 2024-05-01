
using System;
using System.Text;
using OregonTrailDotNet.Window.Travel.Command;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace OregonTrailDotNet.Window.Travel.Dialog
{
    
    [ParentWindow(typeof(Travel))]
    public sealed class TombstoneQuestion : InputForm<TravelInfo>
    {
        
        public TombstoneQuestion(IWindow window) : base(window)
        {
        }

       
        protected override DialogType DialogType => DialogType.YesNo;

        
        protected override string OnDialogPrompt()
        {
            var pointReached = new StringBuilder();

            // Build up message about there being something on the side of the road.
            pointReached.AppendLine(
                $"{Environment.NewLine}You pass a gravesite. Would you");
            pointReached.Append("like to look closer? Y/N");

            return pointReached.ToString();
        }


   
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // Check if the player wants to look at the tombstone or not.
            switch (reponse)
            {
                case DialogResponse.No:
                    SetForm(typeof(ContinueOnTrail));
                    break;
                case DialogResponse.Yes:
                case DialogResponse.Custom:
                    GameSimulationApp.Instance.WindowManager.Add(typeof(Graveyard.Graveyard));

                    // Goes back to continue on trail form below us.
                    ClearForm();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(reponse), reponse, null);
            }
        }
    }
}