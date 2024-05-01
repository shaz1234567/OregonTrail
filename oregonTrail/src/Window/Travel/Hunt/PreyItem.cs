﻿

using OregonTrailDotNet.Entity.Item;
using WolfCurses;

namespace OregonTrailDotNet.Window.Travel.Hunt
{
   
    public sealed class PreyItem : ITick
    {
       
        public PreyItem()
        {
            // Randomly generate a shooting time up to total hunting time.
            Lifetime = 0;
            LifetimeMax = GameSimulationApp.Instance.Random.Next(HuntManager.HUNTINGTIME);

            // Randomly generate maximum amount of time this animal will be a valid target.
            TargetTime = 0;
            TargetTimeMax = GameSimulationApp.Instance.Random.Next(HuntManager.MINTARGETINGTIME,
                HuntManager.MAXTARGETINGTIME);

            // Randomly select a animal for this prey to be from default animals.
            var preyIndex = GameSimulationApp.Instance.Random.Next(HuntManager.DefaultAnimals.Count);

            // Select a random animal that we can be from default animals.
            Animal = new SimItem(HuntManager.DefaultAnimals[preyIndex], 1);
        }

       
        public PreyItem(PreyItem preyItem)
        {
            // Prey specific settings for how long they live on the field.
            Lifetime = preyItem.Lifetime;
            LifetimeMax = preyItem.LifetimeMax;

            // Prey specific for total length of time they may be targeted.
            TargetTime = preyItem.TargetTime;
            TargetTimeMax = preyItem.TargetTimeMax;

            // Copy of the animal from the prey item.
            Animal = new SimItem(preyItem.Animal, 1);
        }

        /// <summary>
        ///     Determines what type of animal this prey will be if the player kills it.
        /// </summary>
        public SimItem Animal { get; }

        /// <summary>
        ///     Determines the total number of seconds this animal has been a valid target for the player to shoot.
        /// </summary>
        public int TargetTime { get; private set; }

        /// <summary>
        ///     Determines the total amount of time that the given prey is allowed the be a target of the player, if this threshold
        ///     is reached the prey will sense the player and run away.
        /// </summary>
        public int TargetTimeMax { get; }

        /// <summary>
        ///     Keeps track of the total number of seconds this prey has been visible. Once it reaches it's maximum shoot time it
        ///     will be removed from the list of possible animals the player can hunt.
        /// </summary>
        public int Lifetime { get; private set; }

        /// <summary>
        ///     Maximum amount of time this animal will be in the list of things that can be killed and are available on the field.
        /// </summary>
        public int LifetimeMax { get; }

        /// <summary>
        ///     Gets or sets the value indicating if the prey target has become aware of the players intentions to shoot them and
        ///     have decided to run away.
        /// </summary>
        public bool ShouldRunAway { get; private set; }

        /// <summary>
        ///     Called when the simulation is ticked by underlying operating system, game engine, or potato. Each of these system
        ///     ticks is called at unpredictable rates, however if not a system tick that means the simulation has processed enough
        ///     of them to fire off event for fixed interval that is set in the core simulation by constant in milliseconds.
        /// </summary>
        /// <remarks>Default is one second or 1000ms.</remarks>
        /// <param name="systemTick">
        ///     TRUE if ticked unpredictably by underlying operating system, game engine, or potato. FALSE if
        ///     pulsed by game simulation at fixed interval.
        /// </param>
        /// <param name="skipDay">
        ///     Determines if the simulation has force ticked without advancing time or down the trail. Used by
        ///     special events that want to simulate passage of time without actually any actual time moving by.
        /// </param>
        public void OnTick(bool systemTick, bool skipDay)
        {
            // No work is done on system ticks.
            if (systemTick)
                return;

            // No work is done if force ticked.
            if (skipDay)
                return;

            // Increment the total shot time until maximum.
            if (Lifetime < LifetimeMax)
                Lifetime++;
            else if (Lifetime > LifetimeMax)
                Lifetime = LifetimeMax;
            else
                Lifetime = LifetimeMax;
        }

        /// <summary>
        ///     Ticks the prey as an active target, it will only count up to randomly determined amount before the prey senses the
        ///     player and runs away.
        /// </summary>
        public void TickTarget()
        {
            // Skip if the target has chosen to flee and run away from the hunter.
            if (ShouldRunAway)
                return;

            // Check if the target time has gone above ceiling.
            if (TargetTime >= TargetTimeMax)
            {
                ShouldRunAway = true;
                TargetTime = TargetTimeMax;
                return;
            }

            // Tick the target time up towards the maximum.
            TargetTime++;
        }
    }
}