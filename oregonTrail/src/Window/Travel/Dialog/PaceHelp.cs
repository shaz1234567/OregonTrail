
using System;
using System.Text;
using OregonTrailDotNet.Window.Travel.Command;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace OregonTrailDotNet.Window.Travel.Dialog
{
    
    [ParentWindow(typeof(Travel))]
    public sealed class PaceHelp : InputForm<TravelInfo>
    {
      
        public PaceHelp(IWindow window) : base(window)
        {
        }

        
     
        protected override string OnDialogPrompt()
        {
            // Steady
            var paceHelp = new StringBuilder();
            paceHelp.Append($"{Environment.NewLine}steady - You travel about 8 hours a{Environment.NewLine}");
            paceHelp.Append($"day, taking frequent rests. You take{Environment.NewLine}");
            paceHelp.Append($"care not to get too tired.{Environment.NewLine}{Environment.NewLine}");

            // Strenuous
            paceHelp.Append($"strenuous - You travel about 12 hours{Environment.NewLine}");
            paceHelp.Append($"a day, starting just after sunrise{Environment.NewLine}");
            paceHelp.Append($"and stopping shortly before sunset.{Environment.NewLine}");
            paceHelp.Append($"You stop to rest only when necessary.{Environment.NewLine}");
            paceHelp.Append($"You finish each day feeling very{Environment.NewLine}");
            paceHelp.Append($"tired.{Environment.NewLine}{Environment.NewLine}");

            // Grueling
            paceHelp.Append($"grueling - You travel about 16 hours{Environment.NewLine}");
            paceHelp.Append($"a day, starting before sunrise and{Environment.NewLine}");
            paceHelp.Append($"continuing until dark. You almost{Environment.NewLine}");
            paceHelp.Append($"never stop to rest. You do not get{Environment.NewLine}");
            paceHelp.Append($"enough sleep at night. You finish{Environment.NewLine}");
            paceHelp.Append($"each day feeling absolutely{Environment.NewLine}");
            paceHelp.Append($"exhausted, and your health suffers.{Environment.NewLine}{Environment.NewLine}");
            return paceHelp.ToString();
        }

        
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // parentGameMode.State = new ChangePace(parentGameMode, UserData);
            SetForm(typeof(ChangePace));
        }
    }
}