

using System;
using System.Text;
using OregonTrailDotNet.Window.MainMenu.Start_Month;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace OregonTrailDotNet.Window.MainMenu.Names
{
    
    [ParentWindow(typeof(MainMenu))]
    public sealed class ConfirmPlayerNames : InputForm<NewGameInfo>
    {
       
        public ConfirmPlayerNames(IWindow window) : base(window)
        {
        }

       
       
        protected override DialogType DialogType => DialogType.Custom;

        
        protected override string OnDialogPrompt()
        {
            // Pass the game data to the simulation for each new game Windows state.
            GameSimulationApp.Instance.SetStartInfo(UserData);

            // Create string builder, counter, print info about party members.
            var confirmPartyText = new StringBuilder();
            confirmPartyText.AppendLine(
                $"{Environment.NewLine}Are these names correct? Y/N{Environment.NewLine}");
            var crewNumber = 1;

            // Loop through every player and print their name.
            for (var index = 0; index < UserData.PlayerNames.Count; index++)
            {
                // First name in list is always the leader.
                var name = UserData.PlayerNames[index];
                var isLeader = (UserData.PlayerNames.IndexOf(name) == 0) && (crewNumber == 1);

                // Only append new line when not printing last line.
                if (index < UserData.PlayerNames.Count - 1)
                    confirmPartyText.AppendLine(isLeader
                        ? $"  {crewNumber} - {name} (leader)"
                        : $"  {crewNumber} - {name}");
                else
                    confirmPartyText.Append($"  {crewNumber} - {name}");

                crewNumber++;
            }

            return confirmPartyText.ToString();
        }

        
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            switch (reponse)
            {
                case DialogResponse.No:
                    RestartNameInput();
                    break;
                case DialogResponse.Yes:
                    UserData.PlayerNameIndex = 0;
                    SetForm(typeof(SelectStartingMonthState));
                    break;
                case DialogResponse.Custom:
                    RestartNameInput();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(reponse), reponse, null);
            }
        }

        
        private void RestartNameInput()
        {
            UserData.PlayerNames.Clear();
            UserData.PlayerNameIndex = 0;
            SetForm(typeof(InputPlayerNames));
        }
    }
}