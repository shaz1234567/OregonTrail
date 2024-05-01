
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OregonTrailDotNet.Entity;
using OregonTrailDotNet.Entity.Item;
using OregonTrailDotNet.Entity.Location;
using WolfCurses;
using WolfCurses.Utility;

namespace OregonTrailDotNet.Window.Travel.Hunt
{
    
    public sealed class HuntManager : ITick
    {
        
        public delegate void TargetFlee(PreyItem target);

       
      
        public const int HUNTINGTIME = 30;

     
        private const int MAXPREY = 15;

        
        public const int MAXFOOD = 100;

        
        public const int MAXTARGETINGTIME = 10;

    
        public const int MINTARGETINGTIME = 3;

      
        private readonly List<PreyItem> _killedPrey;

        
        private readonly List<PreyItem> _preyEscaped;

        
        private int _secondsRemaining;

       
        private readonly List<HuntWord> _shootWords;

        
        private List<PreyItem> _sortedPrey;

      
        private PreyItem _target;

       
        public HuntManager()
        {
            // Clears out any previous killed prey.
            _killedPrey = new List<PreyItem>();
            _sortedPrey = new List<PreyItem>();
            _preyEscaped = new List<PreyItem>();

            // Player has set amount of time in seconds to perform a hunt.
            _secondsRemaining = HUNTINGTIME;

            // Grab all of the shooting words from enum that holds them.
            _shootWords = Enum.GetValues(typeof(HuntWord)).Cast<HuntWord>().ToList();

            // Create animals for the player to shoot with their bullets.
            GeneratePrey();
        }

        
        public HuntWord ShootingWord { get; private set; }

       
        public string HuntInfo
        {
            get
            {
                // Grab instance of game simulation.
                var game = GameSimulationApp.Instance;

                // Will hole on the string data representing hunting status.
                var huntStatus = new StringBuilder();

                // Build up the status for the current hunt.
                huntStatus.AppendLine($"{Environment.NewLine}--------------------------------");

                // Title displays some basic info about the area.
                huntStatus.AppendLine(game.Trail.CurrentLocation.Status != LocationStatus.Departed
                    ? $"Hunting outside {game.Trail.CurrentLocation.Name}"
                    : $"Hunting near {game.Trail.NextLocation.Name}");

                // Represent seconds remaining as daylight left percentage.
                var daylightPercentage = _secondsRemaining/(decimal) HUNTINGTIME;
                huntStatus.AppendLine($"Daylight Remaining: {daylightPercentage*100:N0}%");

                // Current weather on the planes.
                huntStatus.AppendLine($"Weather: {game.Trail.CurrentLocation.Weather.ToDescriptionAttribute()}");
                huntStatus.AppendLine("--------------------------------");

                // Show the player their current shooting word and target they are aiming at.
                huntStatus.AppendLine(
                    $"{Environment.NewLine}Shooting Word: {ShootingWord.ToString().ToUpperInvariant()}");

                // Only show the target to shoot at if there is one.
                huntStatus.AppendLine(_target != null
                    ? $"Target: {_target.Animal.Name.ToUpperInvariant()}{Environment.NewLine}"
                    : "Target: NONE");

                // Targeting time is used to determine when animal will get scared and run away.
                if (_target != null)
                {
                    // Represent targeting time as a percentage of total animal awareness of the hunter.
                    var targetPercentage = _target.TargetTime/(decimal) _target.TargetTimeMax;
                    huntStatus.AppendLine($"Awareness: {targetPercentage*100:N0}%");
                }

                // Prompt the player with information about what to do.
                if (ShootingWord != HuntWord.None)
                {
                    huntStatus.AppendLine($"Type the word '{ShootingWord.ToString().ToLowerInvariant()}' to");
                    huntStatus.Append("take a shot!");
                }
                else
                {
                    // Depending on number of prey change up the wording slightly.
                    var animalText = "animals";
                    if (_sortedPrey.Count == 1)
                        animalText = "animal";

                    // Prey will read out animals for multiple, and just animal for one (1), animals for zero (0).
                    huntStatus.AppendLine(
                        $"{Environment.NewLine}You sense {_sortedPrey.Count:N0} {animalText}");
                    huntStatus.Append("in the area...");
                }

                return huntStatus.ToString();
            }
        }

        
        internal static IList<SimItem> DefaultAnimals
        {
            get
            {
                // Create inventory of items with default starting amounts.
                var defaultAnimals = new List<SimItem>
                {
                    Animals.Bear,
                    Animals.Buffalo,
                    Animals.Caribou,
                    Animals.Deer,
                    Animals.Duck,
                    Animals.Goose,
                    Animals.Rabbit,
                    Animals.Squirrel
                };

                // Zero out all of the quantities by removing their max quantity.
                foreach (var animal in defaultAnimals)
                    animal.ReduceQuantity(animal.MaxQuantity);

                // Now we have default animals for hunting with all quantities zeroed out.
                return defaultAnimals;
            }
        }

    
        public bool PreyAvailable => ShootingWord != HuntWord.None;

      
        public bool ShouldEndHunt => _secondsRemaining <= 0;

        
        public int KillWeight
        {
            get
            {
                // Skip if there are no prey items.
                if (_killedPrey.Count <= 0)
                    return 0;

                // Loop through every killed prey and tabulate total weight.
                var totalWeight = 0;
                foreach (var preyItem in _killedPrey)
                    totalWeight += preyItem.Animal.TotalWeight;

                return totalWeight;
            }
        }

       
        public PreyItem LastEscapee => _preyEscaped.LastOrDefault();

        
        public PreyItem LastTarget => _killedPrey.LastOrDefault();

        
        public void OnTick(bool systemTick, bool skipDay)
        {
            // No work is done on system ticks.
            if (systemTick)
                return;

            // No work is done if force ticked.
            if (skipDay)
                return;

            // Check if we are still allowed to hunt.
            if (_secondsRemaining <= 0)
                return;

            // Remove one (1) second from the total remaining hunting time.
            _secondsRemaining--;

            // Increments timer on targets prey increasing the chance they will run away.
            TickTargetAwareness();

            // Advances the lifetime of each prey object in the list.
            TickPrey();

            // Pick a random shooting word, and if not none, an animal for prey target.
            TryPickPrey();
        }

        
        private void TickPrey()
        {
            // Loop through every sorted prey and check lifetime.
            var copyPrey = new List<PreyItem>(_sortedPrey);
            foreach (var prey in copyPrey)
                if (prey.Lifetime >= prey.LifetimeMax)
                    _sortedPrey.Remove(prey);
                else
                    prey.OnTick(false, false);

            // Cleanup copied list of prey for iteration.
            copyPrey.Clear();
        }

      
        private void TickTargetAwareness()
        {
            // Check if there is a target at all to tick.
            if (_target == null)
                return;

            // There is a change we will not tick awareness this time.
            if (GameSimulationApp.Instance.Random.NextBool())
                return;

            // Check target ticking if not null and shooting word not none.
            if ((_target != null) && (ShootingWord != HuntWord.None))
                _target.TickTarget();

            // Check if the target wants to run away from the hunter.
            if (!_target.ShouldRunAway)
                return;

            // Add the prey to list of things that got away.
            _preyEscaped.Add(new PreyItem(_target));

            // Fire event that hunting mode will attach form in response to.
            TargetFledEvent?.Invoke(_target);

            // Reset target and shooting word.
            ClearTarget();
        }

     
        private void ClearTarget()
        {
            // Clear the target.
            _target = null;

            // Set the shooting word back to none.
            ShootingWord = HuntWord.None;

            // Clear the input buffer.
            GameSimulationApp.Instance.InputManager.ClearBuffer();
        }
    
        private void TryPickPrey()
        {
            // Skip if we already have a target.
            if (_target != null)
                return;

            // Check if there is any prey we are currently hunting.
            if (_sortedPrey.Count <= 0)
                return;

            // There is a chance that you will not get prey this tick.
            if (GameSimulationApp.Instance.Random.NextBool())
                return;

            // Randomly select one of the hunting words from the list.
            var tempShootWord = (HuntWord) GameSimulationApp.Instance.Random.Next(_shootWords.Count);

            // Check if we are already trying to hunt a particular animal.
            if ((tempShootWord == HuntWord.None) || (tempShootWord == ShootingWord))
                return;

            // Set the shooting word to the one we have now verified. 
            ShootingWord = tempShootWord;

            // Randomly select one of the prey from the list.
            var randomPreyIndex = GameSimulationApp.Instance.Random.Next(_sortedPrey.Count);
            var randomPrey = _sortedPrey[randomPreyIndex];

            // Check the prey to make sure it is still alive.
            if (randomPrey.Lifetime > randomPrey.LifetimeMax)
                return;

            // Set the verified prey as hunting target.
            _target = new PreyItem(randomPrey);

            // Remove the old prey from the list now that it is a target.
            _sortedPrey.Remove(randomPrey);
        }

        /// <summary>
        ///     Generate random number of animals to occupy this area.
        /// </summary>
        private void GeneratePrey()
        {
            // Check to make sure spawn count is above zero.
            var preySpawnCount = GameSimulationApp.Instance.Random.Next(MAXPREY);
            if (preySpawnCount <= 0)
                return;

            // Create the number of prey required by the dice roll.
            var unsortedPrey = new List<PreyItem>();
            for (var i = 0; i < preySpawnCount; i++)
                unsortedPrey.Add(new PreyItem());

            // Sort the list of references in memory without creating duplicate objects.
            _sortedPrey = unsortedPrey.OrderByDescending(o => o.LifetimeMax).Distinct().ToList();
        }

        
        public event TargetFlee TargetFledEvent;

    
        public bool TryShoot()
        {
            // Skip there is no valid target to shoot at.
            if (_target == null)
                return false;

            // Grab game instance to make check logic legible.
            var game = GameSimulationApp.Instance;

            // Check if the player outright missed their target, banker is way worse than farmer.
            if (100*game.Random.Next() < ((int) game.Vehicle.PassengerLeader.Profession - 13)*_target.TargetTime)
            {
                _preyEscaped.Add(_target);
                ClearTarget();
                return false;
            }

            // Check if player fired in less than half the maximum target time for this prey.
            if (_target.TargetTime > _target.TargetTimeMax/2)
            {
                _preyEscaped.Add(_target);
                ClearTarget();
                return false;
            }

            // Calculate the total cost of this shot in bullets.
            var bulletCost = (int) game.Vehicle.Inventory[Entities.Ammo].TotalValue - 10 -
                             game.Random.Next()*4;

            // Remove the amount of bullets from vehicle inventory.
            game.Vehicle.Inventory[Entities.Ammo].ReduceQuantity(bulletCost);

            // Add the target to the list of animals that have been killed.
            _killedPrey.Add(_target);

            // Resets the targeting system now that the animal is bagged and tagged.
            ClearTarget();
            return true;
        }
    }
}