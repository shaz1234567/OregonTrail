

using System;
using System.Collections.Generic;
using System.Text;
using OregonTrailDotNet.Entity.Item;
using OregonTrailDotNet.Module.Scoring;
using WolfCurses.Window;
using WolfCurses.Window.Control;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace OregonTrailDotNet.Window.MainMenu.Help
{
    
    [ParentWindow(typeof(MainMenu))]
    public sealed class PointsAwardHelp : InputForm<NewGameInfo>
    {
       
       
        public PointsAwardHelp(IWindow window) : base(window)
        {
        }

       
        private static IEnumerable<Points> ResourcePoints => new List<Points>
        {
            new Points(Resources.Person),
            new Points(Resources.Vehicle),
            new Points(Parts.Oxen),
            new Points(Parts.Wheel),
            new Points(Parts.Axle),
            new Points(Parts.Tongue),
            new Points(Resources.Clothing),
            new Points(Resources.Bullets),
            new Points(Resources.Food),
            new Points(Resources.Cash)
        };

       
       
        protected override string OnDialogPrompt()
        {
            var pointsItems = new StringBuilder();
            pointsItems.Append($"{Environment.NewLine}On Arriving in Oregon{Environment.NewLine}{Environment.NewLine}");
            pointsItems.Append($"The resources you arrive with will{Environment.NewLine}");
            pointsItems.Append($"help you get started in the new{Environment.NewLine}");
            pointsItems.Append($"land. You receive points for each{Environment.NewLine}");
            pointsItems.Append($"item you bring safely to Oregon.{Environment.NewLine}{Environment.NewLine}");

            // Build up the table of resource points and how they work for player.
            var partyTable = ResourcePoints.ToStringTable(
                new[] {"Resources of Party", "Points per Item"},
                u => u.ToString(),
                u => u.PointsAwarded
            );

            // Print the table of how resources earn points.
            pointsItems.AppendLine(partyTable);
            return pointsItems.ToString();
        }

       
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // parentGameMode.State = new PointsMultiplyerHelp(parentGameMode, UserData);
            SetForm(typeof(PointsMultiplyerHelp));
        }
    }
}