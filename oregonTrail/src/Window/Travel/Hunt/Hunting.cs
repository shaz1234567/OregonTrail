

using System;
using OregonTrailDotNet.Window.Travel.Hunt.Help;
using WolfCurses.Window;
using WolfCurses.Window.Form;

namespace OregonTrailDotNet.Window.Travel.Hunt
{
    
    [ParentWindow(typeof(Travel))]
    public sealed class Hunting : Form<TravelInfo>
    {
      
        public Hunting(IWindow window) : base(window)
        {
        }

        
        public override bool InputFillsBuffer => UserData.Hunt.PreyAvailable;

       
        public override bool AllowInput => UserData.Hunt.PreyAvailable;

     
        public override void OnFormPostCreate()
        {
            base.OnFormPostCreate();

            // Listen to hunting manager for event about targeting prey running away.
            UserData.Hunt.TargetFledEvent += Hunt_TargetFledEvent;
        }

        
        private void Hunt_TargetFledEvent(PreyItem target)
        {
            SetForm(typeof(PreyFlee));
        }

        
        public override void OnTick(bool systemTick, bool skipDay)
        {
            base.OnTick(systemTick, skipDay);

            // Depending on the state of the hunt we will keep ticking or show result of players efforts for the session.
            if (UserData.Hunt.ShouldEndHunt)
            {
                // Unhook event for when targeted prey flees.
                UserData.Hunt.TargetFledEvent -= Hunt_TargetFledEvent;

                // Attach the hunting result form.
                SetForm(typeof(HuntingResult));
            }
            else
            {
                // Tick the hunting session normally.
                UserData.Hunt?.OnTick(systemTick, skipDay);
            }
        }

     
        public override string OnRenderForm()
        {
            return UserData.Hunt.HuntInfo;
        }

       
        public override void OnInputBufferReturned(string input)
        {
            // Skip if the input is null or empty.
            if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
                return;

            // Check if we have anything to shoot at right now.
            if (!UserData.Hunt.PreyAvailable)
                return;

            // Check if the user spelled the shooting word correctly.
            if (!input.Equals(UserData.Hunt.ShootingWord.ToString(), StringComparison.OrdinalIgnoreCase))
                return;

            // Determine if the player shot an animal or missed their shot.
            SetForm(UserData.Hunt.TryShoot() ? typeof(PreyHit) : typeof(PreyMissed));
        }
    }
}