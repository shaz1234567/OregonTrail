

using System;
using System.Text;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace OregonTrailDotNet.Window.MainMenu.Help
{
    
    [ParentWindow(typeof(MainMenu))]
    public sealed class StoreHelp : InputForm<NewGameInfo>
    {
       
        public StoreHelp(IWindow window) : base(window)
        {
        }

        
        protected override string OnDialogPrompt()
        {
            var storeHelp = new StringBuilder();
            storeHelp.Append($"{Environment.NewLine}You can buy whatever you need at{Environment.NewLine}");
            storeHelp.Append($"Matt's General Store.{Environment.NewLine}{Environment.NewLine}");
            return storeHelp.ToString();
        }

        
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // Closes main menu and drops back to travel Windows at the bottom level which should have store already open and ready.
            ParentWindow.RemoveWindowNextTick();
        }
    }
}