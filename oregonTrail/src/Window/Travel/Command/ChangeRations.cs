

using System;
using System.Text;
using OregonTrailDotNet.Entity.Person;
using WolfCurses.Utility;
using WolfCurses.Window;
using WolfCurses.Window.Form;

namespace OregonTrailDotNet.Window.Travel.Command
{
   
    [ParentWindow(typeof(Travel))]
    public sealed class ChangeRations : Form<TravelInfo>
    {
      
        private StringBuilder _ration;

       
        public ChangeRations(IWindow window) : base(window)
        {
        }

      
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();

            _ration = new StringBuilder();
            _ration.AppendLine($"{Environment.NewLine}Change food rations");
            _ration.AppendLine(
                $"(currently \"{GameSimulationApp.Instance.Vehicle.Ration.ToDescriptionAttribute()}\"){Environment.NewLine}");
            _ration.AppendLine("The amount of food the people in");
            _ration.AppendLine("your party eat each day can");
            _ration.AppendLine($"change. These amounts are:{Environment.NewLine}");
            _ration.AppendLine("1. filling - meals are large and");
            _ration.AppendLine($"   generous.{Environment.NewLine}");
            _ration.AppendLine("2. meager - meals are small, but");
            _ration.AppendLine($"   adequate.{Environment.NewLine}");
            _ration.AppendLine("3. bare bones - meals are very");
            _ration.Append("   small, everyone stays hungry.");
        }

        
        public override string OnRenderForm()
        {
            return _ration.ToString();
        }

       
        public override void OnInputBufferReturned(string input)
        {
            switch (input.ToUpperInvariant())
            {
                case "1":
                    GameSimulationApp.Instance.Vehicle.ChangeRations(RationLevel.Filling);
                    ClearForm();
                    break;
                case "2":
                    GameSimulationApp.Instance.Vehicle.ChangeRations(RationLevel.Meager);
                    ClearForm();
                    break;
                case "3":
                    GameSimulationApp.Instance.Vehicle.ChangeRations(RationLevel.BareBones);
                    ClearForm();
                    break;
                default:
                    SetForm(typeof(ChangeRations));
                    break;
            }
        }
    }
}