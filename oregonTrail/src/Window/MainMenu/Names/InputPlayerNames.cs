
using System;
using System.Text;
using WolfCurses.Window;
using WolfCurses.Window.Form;

namespace OregonTrailDotNet.Window.MainMenu.Names
{
   
    [ParentWindow(typeof(MainMenu))]
    public sealed class InputPlayerNames : Form<NewGameInfo>
    {
       
        private StringBuilder _inputNamesHelp;

       
        public InputPlayerNames(IWindow window) : base(window)
        {
        }

       
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();

            // Pass the game data to the simulation for each new game Windows state.
            GameSimulationApp.Instance.SetStartInfo(UserData);

            // Create string builder so we only build up this data once.
            _inputNamesHelp = new StringBuilder();

          
            switch (UserData.PlayerNameIndex)
            {
                case 0:
                    _inputNamesHelp.Append(Environment.NewLine +
                                           $"{MainMenu.LEADER_QUESTION}");
                    break;
                case 1:
                    _inputNamesHelp.Append(Environment.NewLine +
                                           $"{MainMenu.MEMBERS_QUESTION}" +
                                           $"{Environment.NewLine}{Environment.NewLine}");
                    break;
                case 2:
                    _inputNamesHelp.Append(Environment.NewLine +
                                           $"{MainMenu.MEMBERS_QUESTION}" +
                                           $"{Environment.NewLine}{Environment.NewLine}");
                    break;
                case 3:
                    _inputNamesHelp.Append(Environment.NewLine +
                                           $"{MainMenu.MEMBERS_QUESTION}" +
                                           $"{Environment.NewLine}{Environment.NewLine}");
                    break;
            }

            // Only print player names if we have some to actually print.
            if (UserData.PlayerNames.Count > 0)
            {
                // Loop through all the player names and get their current state.
                var crewNumber = 1;

                // Loop through every player and print their name.
                for (var index = 0; index < GameSimulationApp.MAXPLAYERS; index++)
                {
                    var name = string.Empty;
                    if (index < UserData.PlayerNames.Count)
                        name = UserData.PlayerNames[index];

                    // First name in list is always the leader.
                    var isLeader = (UserData.PlayerNames.IndexOf(name) == 0) && (crewNumber == 1);
                    _inputNamesHelp.AppendFormat(isLeader
                        ? $"  {crewNumber} - {name} (leader){Environment.NewLine}"
                        : $"  {crewNumber} - {name}{Environment.NewLine}");
                    crewNumber++;
                }

                // Wait for user input...
                _inputNamesHelp.Append("\n(Enter names or press Enter)");
            }
        }

        /// <summary>
      
        public override string OnRenderForm()
        {
            return _inputNamesHelp.ToString();
        }

        
        public override void OnInputBufferReturned(string input)
        {
            // If player enters empty name fill out all the slots with random ones.
            if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
            {
                // Only fill out names for slots that are empty.
                for (var i = 0; i < GameSimulationApp.MAXPLAYERS - UserData.PlayerNameIndex; i++)
                    UserData.PlayerNames.Insert(UserData.PlayerNameIndex, GetPlayerName());

                // Attach state to confirm randomized name selection, skipping manual entry with the return.
                SetForm(typeof(ConfirmPlayerNames));
                return;
            }

            // Add the name to list since we will have something at this point even if randomly generated.
            UserData.PlayerNames.Insert(UserData.PlayerNameIndex, input);
            UserData.PlayerNameIndex++;

            // Change the state to either confirm or input the next name based on index of name we are entering.
            SetForm(UserData.PlayerNameIndex < GameSimulationApp.MAXPLAYERS
                ? typeof(InputPlayerNames)
                : typeof(ConfirmPlayerNames));
        }

      
        private static string GetPlayerName()
        {
            string[] names =
            {
                "Bob", "Joe", "Sally", "Tim", "Steve", "Zeke", "Suzan", "Rebekah", "Young", "Marquitta",
                "Kristy", "Sharice", "Joanna", "Chrystal", "Genevie", "Angela", "Ruthann", "Viva", "Iris", "Anderson",
                "Siobhan", "Karey", "Jolie", "Carlene", "Lekisha", "Buck"
            };
            return names[GameSimulationApp.Instance.Random.Next(names.Length)];
        }
    }
}