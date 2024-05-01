

using System;
using System.Text;
using OregonTrailDotNet.Window.MainMenu.Help;
using OregonTrailDotNet.Window.MainMenu.Options;
using OregonTrailDotNet.Window.MainMenu.Profession;
using WolfCurses;
using WolfCurses.Window;

namespace OregonTrailDotNet.Window.MainMenu
{
   
    public sealed class MainMenu : Window<MainMenuCommands, NewGameInfo>
    {
      
        public const string LEADER_QUESTION = "What is the first name of the wagon leader?";

        
        public static readonly string MEMBERS_QUESTION =
            $"What are the first names of the{Environment.NewLine}three other members in your party?";

      
        public MainMenu(SimulationApp simUnit) : base(simUnit)
        {
        }

        
        public override void OnWindowPostCreate()
        {
            var headerText = new StringBuilder();
            headerText.Append($"{Environment.NewLine}The Oregon Trail{Environment.NewLine}{Environment.NewLine}");
            headerText.Append("You may:");
            MenuHeader = headerText.ToString();

            AddCommand(TravelTheTrail, MainMenuCommands.TravelTheTrail);
            AddCommand(LearnAboutTrail, MainMenuCommands.LearnAboutTheTrail);
            AddCommand(SeeTopTen, MainMenuCommands.SeeTheOregonTopTen);
            AddCommand(ChooseManagementOptions, MainMenuCommands.ChooseManagementOptions);
            AddCommand(CloseSimulation, MainMenuCommands.CloseSimulation);
        }

        
        private static void CloseSimulation()
        {
            GameSimulationApp.Instance.Destroy();
        }

     
        private void ChooseManagementOptions()
        {
            SetForm(typeof(ManagementOptions));
        }

        
        private void SeeTopTen()
        {
            SetForm(typeof(CurrentTopTen));
        }

        
        private void LearnAboutTrail()
        {
            SetForm(typeof(RulesHelp));
        }

       
        private void TravelTheTrail()
        {
            SetForm(typeof(ProfessionSelector));
        }
    }
}