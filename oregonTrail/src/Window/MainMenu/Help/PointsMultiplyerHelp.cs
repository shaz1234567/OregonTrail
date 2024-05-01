

using System;
using System.Text;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace OregonTrailDotNet.Window.MainMenu.Help
{
   
    [ParentWindow(typeof(MainMenu))]
    public sealed class PointsMultiplyerHelp : InputForm<NewGameInfo>
    {
        
       
        public PointsMultiplyerHelp(IWindow window) : base(window)
        {
        }

        
        protected override string OnDialogPrompt()
        {
            var pointsProfession = new StringBuilder();
            pointsProfession.Append(
                $"{Environment.NewLine}On Arriving in Oregon{Environment.NewLine}{Environment.NewLine}");
            pointsProfession.AppendLine("You receive points for your");
            pointsProfession.AppendLine("occupation in the new land.");
            pointsProfession.AppendLine("Because more farmers and");
            pointsProfession.AppendLine("carpenters were needed than");
            pointsProfession.AppendLine("bankers, you receive double");
            pointsProfession.AppendLine("points upon arriving in Oregon");
            pointsProfession.AppendLine("as a carpenter, and triple");
            pointsProfession.AppendLine($"points for arriving as a farmer.{Environment.NewLine}");
            return pointsProfession.ToString();
        }

      
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            ClearForm();
        }
    }
}