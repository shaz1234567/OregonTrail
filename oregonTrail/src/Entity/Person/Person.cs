

using System;
using OregonTrailDotNet.Entity.Vehicle;
using OregonTrailDotNet.Event;
using OregonTrailDotNet.Event.Person;

namespace OregonTrailDotNet.Entity.Person
{
    
    public sealed class Person : IEntity
    {
        
        private int _status;

       
        private bool _dead;

       
        private bool _nearDeathExperience;

   
        public Person(Profession profession, string name, bool leader)
        {
            // Person needs a name, profession, and need to know if they are the leader.
            Profession = profession;
            Name = name;
            Leader = leader;

            // Person starts with clean bill of health.
            Infected = false;
            Injured = false;
            Status = (int) HealthStatus.Good;
        }

       
        private bool Infected { get; set; }

       
        public HealthStatus HealthStatus
        {
            get
            {
                // Skip if this person is dead, cannot heal them.
                if (_dead)
                {
                    _status = (int) HealthStatus.Dead;
                    return HealthStatus.Dead;
                }

                // Health is greater than fair so it must be good.
                if (Status > (int) HealthStatus.Fair)
                    return HealthStatus.Good;

                // Health is less than good, but greater than poor so it must be fair.
                if ((Status < (int) HealthStatus.Good) && (Status > (int) HealthStatus.Poor))
                    return HealthStatus.Fair;

                // Health is less than fair, but greater than very poor so it is just poor.
                if ((Status < (int) HealthStatus.Fair) && (Status > (int) HealthStatus.VeryPoor))
                    return HealthStatus.Poor;

                // Health is less than poor, but not quite dead yet so it must be very poor.
                if ((Status < (int) HealthStatus.Poor) && (Status > (int) HealthStatus.Dead))
                    return HealthStatus.VeryPoor;

                // Default response is to indicate this person is dead.
                return HealthStatus.Dead;
            }
        }

      
        private int Status
        {
            get => _status;
            set
            {
                // Skip if this person is dead, cannot heal them.
                if (_dead)
                {
                    _status = (int) HealthStatus.Dead;
                    return;
                }

                // Check that value is not above max.
                if (value >= (int) HealthStatus.Good)
                {
                    _status = (int) HealthStatus.Good;
                    return;
                }

                // Check that value is not below min.
                if (value <= (int) HealthStatus.Dead)
                {
                    _dead = true;
                    _status = (int) HealthStatus.Dead;
                    return;
                }

                // Set health to ceiling corrected value.
                _status = value;
            }
        }

    
        public Profession Profession { get; }

        public bool Leader { get; }

        
        private bool Injured { get; set; }

       
        public string Name { get; }

       
        public int Compare(IEntity x, IEntity y)
        {
            var result = string.Compare(x?.Name, y?.Name, StringComparison.Ordinal);
            if (result != 0) return result;

            return result;
        }

        
        public int CompareTo(IEntity other)
        {
            var result = string.Compare(other.Name, Name, StringComparison.Ordinal);
            return result;
        }

        
        public bool Equals(IEntity other)
        {
            // Reference equality check
            if (this == other)
                return true;

            if (other == null)
                return false;

            if (other.GetType() != GetType())
                return false;

            if (Name.Equals(other.Name))
                return true;

            return false;
        }

        
        public bool Equals(IEntity x, IEntity y)
        {
            return x.Equals(y);
        }

       
        public int GetHashCode(IEntity obj)
        {
            var hash = 23;
            hash = hash*31 + Name.GetHashCode();
            return hash;
        }

        public void OnTick(bool systemTick, bool skipDay)
        {
            // Only tick person with simulation.
            if (systemTick)
                return;

            // Skip if this person is dead, cannot heal them.
            if ((HealthStatus == HealthStatus.Dead) || _dead)
                return;

            // Grab instance of the game simulation to increase readability.
            var game = GameSimulationApp.Instance;

            // Eating poorly raises risk of illness.
            if (game.Vehicle.Ration == RationLevel.BareBones)
                CheckIllness();
            else if ((game.Vehicle.Ration == RationLevel.Meager) &&
                     game.Random.NextBool())
                CheckIllness();

            // More change for illness if you have no clothes.
            var costClothes = game.Vehicle.Inventory[Entities.Clothes].TotalValue;
            if (costClothes > 22 + 4*game.Random.Next())
            {
                CheckIllness();
            }
            else
            {
                // Random chance for illness in general, even with nice clothes but much lower.
                if (game.Random.NextBool() && game.Random.NextBool())
                    CheckIllness();
            }

            // Will only consume food if a whole day goes by, realtime actions won't have this penalty.
            if (!skipDay)
                ConsumeFood();
        }

        
        private void ConsumeFood()
        {
            // Skip if this person is dead, cannot heal them.
            if ((HealthStatus == HealthStatus.Dead) || _dead)
                return;

            // Grab instance of the game simulation to increase readability.
            var game = GameSimulationApp.Instance;

            // Check if player has any food to eat.
            if (game.Vehicle.Inventory[Entities.Food].Quantity > 0)
            {
                // Consume some food based on ration level, then update the cost to check against.
                game.Vehicle.Inventory[Entities.Food].ReduceQuantity((int) game.Vehicle.Ration*
                                                                     game.Vehicle.PassengerLivingCount);

                // Change to get better when eating well.
                Heal();
            }
            else
            {
                // Reduce the players health until they are dead.
                Damage(10, 50);
            }
        }

        
        private void Heal()
        {
            // Skip if this person is dead, cannot heal them.
            if ((HealthStatus == HealthStatus.Dead) || _dead)
                return;

            // Skip if already at max health.
            if (HealthStatus == HealthStatus.Good)
                return;

            // Grab instance of the game simulation to increase readability.
            var game = GameSimulationApp.Instance;

            // Person will not get healed every single time it is possible to do so.
            if (game.Random.NextBool())
                return;

            // Check if the player has made a recovery from near death.
            if (_nearDeathExperience && (Infected || Injured))
            {
                // We only want to show the well again event if the player made a massive recovery.
                _nearDeathExperience = false;

                // Roll the dice, person can get better or way worse here.
                game.EventDirector.TriggerEvent(this, game.Random.NextBool()
                    ? typeof(WellAgain)
                    : typeof(TurnForWorse));
            }
            else
            {
                // Increase health by a random amount.
                Status += game.Random.Next(1, 10);
            }
        }

       
        public void HealEntirely()
        {
            Status = (int) HealthStatus.Good;
            Infected = false;
            Injured = false;
        }

    
        private void CheckIllness()
        {
            // Grab instance of the game simulation to increase readability.
            var game = GameSimulationApp.Instance;

            // Cannot calculate illness for the dead.
            if ((HealthStatus == HealthStatus.Dead) || _dead)
                return;

            // Person will not get hurt every single time it is called.
            if (game.Random.NextBool())
                return;

            if (game.Random.Next(100) <= 10 +
                35*((int) game.Vehicle.Ration - 1))
            {
                // Mild illness.
                game.Vehicle.ReduceMileage(5);
                Damage(10, 50);
            }
            else if (game.Random.Next(100) <= 5 -
                     40/game.Vehicle.Passengers.Count*
                     ((int) game.Vehicle.Ration - 1))
            {
                // Severe illness.
                game.Vehicle.ReduceMileage(15);
                Damage(10, 50);
            }

            // Determines if we should roll for infections based on previous complications.
            switch (HealthStatus)
            {
                case HealthStatus.Good:
                    if ((Infected || Injured) && (game.Vehicle.Status != VehicleStatus.Stopped))
                    {
                        game.Vehicle.ReduceMileage(5);
                        Damage(10, 50);
                    }

                    break;
                case HealthStatus.Fair:
                    if ((Infected || Injured) && (game.Vehicle.Status != VehicleStatus.Stopped))
                        if (game.Random.NextBool())
                        {
                            // Hurt the player and reduce total possible mileage this turn.
                            game.Vehicle.ReduceMileage(5);
                            Damage(10, 50);
                        }
                        else if ((!Infected || !Injured) && (game.Vehicle.Status == VehicleStatus.Stopped))
                        {
                            // Heal the player if their health is below good with no infections or injures.
                            Heal();
                        }

                    break;
                case HealthStatus.Poor:
                    if ((Infected || Injured) && (game.Vehicle.Status != VehicleStatus.Stopped))
                    {
                        game.Vehicle.ReduceMileage(10);
                        Damage(5, 10);
                    }

                    break;
                case HealthStatus.VeryPoor:
                    _nearDeathExperience = true;
                    game.Vehicle.ReduceMileage(15);
                    Damage(1, 5);
                    break;
                case HealthStatus.Dead:
                    _dead = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

      
        private void Damage(int minAmount, int maxAmount)
        {
            // Skip what is already dead, no damage to be applied.
            if (HealthStatus == HealthStatus.Dead)
                return;

            // Grab instance of the game simulation to increase readability.
            var game = GameSimulationApp.Instance;

            // Reduce the persons health by random amount from death amount to desired damage level.
            Status -= game.Random.Next(minAmount, maxAmount);

            // Chance for broken bones and other ailments related to damage (but not death).
            if (!Infected || !Injured)
                game.EventDirector.TriggerEventByType(this, EventCategory.Person);

            // Check if health dropped to dead levels.
            if (HealthStatus != HealthStatus.Dead)
                return;

            // Reduce person's health to dead level.
            Status = (int) HealthStatus.Dead;

            // Check if leader died or party member and execute corresponding event.
            game.EventDirector.TriggerEvent(this, Leader ? typeof(DeathPlayer) : typeof(DeathCompanion));
        }

        
        public void Damage(int amount)
        {
            // Skip if the amount is less than or equal to zero.
            if (amount <= 0)
                return;

            // Remove the health from the person.
            Status -= amount;
        }

       
        public void Kill()
        {
            // Skip if the person is already dead.
            if (HealthStatus == HealthStatus.Dead)
                return;

            // Ashes to ashes, dust to dust...
            Status = 0;
        }

       
        public void Infect()
        {
            Infected = true;
        }

       
        public void Injure()
        {
            Injured = true;
        }
    }
}