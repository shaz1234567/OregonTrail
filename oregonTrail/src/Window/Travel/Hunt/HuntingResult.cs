
using System;
using System.Text;
using OregonTrailDotNet.Entity;
using WolfCurses.Window;
using WolfCurses.Window.Form;
using WolfCurses.Window.Form.Input;

namespace OregonTrailDotNet.Window.Travel.Hunt
{
   
    [ParentWindow(typeof(Travel))]
    public sealed class HuntingResult : InputForm<TravelInfo>
    {
       
        private int _finalKillWeight;

      
        private readonly StringBuilder _huntScore;

     
        public HuntingResult(IWindow window) : base(window)
        {
            _huntScore = new StringBuilder();
        }

       
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();

            // After hunting we roll the dice on the party and player and skip a day.
            GameSimulationApp.Instance.TakeTurn(false);
        }

      
        protected override string OnDialogPrompt()
        {
            // Clear previous hunting score information.
            _huntScore.Clear();

            // Calculate total weight of all animals killed by player during hunt.
            var killWeight = UserData.Hunt.KillWeight;

            // Depending on kill weight we change response and message.
            if (killWeight <= 0)
            {
                _huntScore.AppendLine($"{Environment.NewLine}You were unable to shoot any");
                _huntScore.AppendLine($"food.{Environment.NewLine}");
            }
            else if (killWeight > 0)
            {
                // Message to let the player know they killed prey.
                _huntScore.AppendLine($"{Environment.NewLine}From the animals you shot, you");
                _huntScore.AppendLine($"got {killWeight:N0} pounds of meat.{Environment.NewLine}");

                // Adds the killing weight since it is safe at this point.
                _finalKillWeight = killWeight;

                // Player can only take MAXFOOD amount from hunt regardless of total weight.
                if (killWeight <= HuntManager.MAXFOOD)
                    return _huntScore.ToString();

                // Forces the weight of the kill to become
                _finalKillWeight = HuntManager.MAXFOOD;

                // Player killed to many animals.
                _huntScore.AppendLine("However, you were only able to");
                _huntScore.AppendLine($"carry {_finalKillWeight:N0} pounds back to the");
                _huntScore.AppendLine($"wagon.{Environment.NewLine}");
            }

            // Return the hunting result to text renderer.
            return _huntScore.ToString();
        }

      
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            // Transfers the total finalized kill weight we calculated to vehicle inventory as food in pounds.
            if (_finalKillWeight > 0)
                GameSimulationApp.Instance.Vehicle.Inventory[Entities.Food].AddQuantity(_finalKillWeight);

            // Destroys all hunting related data now that we are done with it.
            UserData.DestroyHunt();

            // Returns to the travel menu so the player can continue down the trail.
            ClearForm();
        }
    }
}