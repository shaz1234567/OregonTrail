
using System;
using WolfCurses.Window;
using WolfCurses.Window.Form;

namespace OregonTrailDotNet.Window.Travel.Rest
{
   
    [ParentWindow(typeof(Travel))]
    public sealed class RestAmount : Form<TravelInfo>
    {
      
        public RestAmount(IWindow window) : base(window)
        {
        }

       
        public override string OnRenderForm()
        {
            return Environment.NewLine + "How many days would you like to rest?";
        }

      
        public override void OnInputBufferReturned(string input)
        {
            // Parse the user input buffer as a unsigned int.
            if (!int.TryParse(input, out var parsedInputNumber))
                return;

            // For diversion, or out of necessity, a party can rest from one to nine days.
            UserData.DaysToRest = parsedInputNumber;
            if ((parsedInputNumber > 0) && (parsedInputNumber <= 9))
                SetForm(typeof(Resting));
            else
                ClearForm();
        }
    }
}