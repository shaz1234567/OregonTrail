

using System;
using System.Collections.Generic;
using System.Text;
using OregonTrailDotNet.Entity.Person;
using WolfCurses.Utility;
using WolfCurses.Window;
using WolfCurses.Window.Control;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace OregonTrailDotNet.Window.MainMenu.Help
{

    [ParentWindow(typeof(MainMenu))]
    public sealed class PointsDistributionHelp : InputForm<NewGameInfo>
    {
       
        public PointsDistributionHelp(IWindow window) : base(window)
        {
        }

       
        protected override string OnDialogPrompt()
        {
            // Build up string of help about points for people.
            var pointsHealth = new StringBuilder();
            pointsHealth.AppendLine($"{Environment.NewLine}On Arriving in Oregon{Environment.NewLine}");
            pointsHealth.AppendLine("Your most important resource is the");
            pointsHealth.AppendLine("people you have with you. You");
            pointsHealth.AppendLine("receive points for each member of");
            pointsHealth.AppendLine("your party who arrives safely; you");
            pointsHealth.AppendLine("receive more points if they arrive");
            pointsHealth.AppendLine($"in good health!{Environment.NewLine}");

            // Repair status reference dictionary.
            var repairLevels = new Dictionary<string, int>();
            foreach (var repairStat in Enum.GetNames(typeof(HealthStatus)))
                repairLevels.Add(repairStat, (int) Enum.Parse(typeof(HealthStatus), repairStat));

            // Build a text table from people point distribution with custom headers.
            var partyTextTable = repairLevels.Values.ToStringTable(
                new[] {"HealthStatus of Party", "Points per Person"},
                u => Enum.Parse(typeof(HealthStatus), u.ToString()).ToDescriptionAttribute(),
                u => u);

            // Print the table to the screen buffer.
            pointsHealth.AppendLine(partyTextTable);
            return pointsHealth.ToString();
        }

        
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // parentGameMode.State = new PointsAwardHelp(parentGameMode, UserData);
            SetForm(typeof(PointsAwardHelp));
        }
    }
}