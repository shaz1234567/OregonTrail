

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OregonTrailDotNet.Entity.Item;
using OregonTrailDotNet.Entity.Person;
using OregonTrailDotNet.Event;

namespace OregonTrailDotNet.Entity.Vehicle
{
    
    public sealed class Vehicle : IEntity
    {
        
        private Dictionary<Entities, SimItem> _inventory;

        
        private List<SimItem> _parts;

       
        private List<Person.Person> _passengers;

       
        public Vehicle()
        {
            ResetVehicle();
            Name = "Vehicle";
            Pace = TravelPace.Steady;
            Mileage = 1;
            Status = VehicleStatus.Stopped;
        }

        public bool PassengersDead
        {
            get
            {
                // Everybody cannot be dead if nobody is there!
                if (Passengers.Count <= 0)
                    return false;

                // Loop through all passengers and add their death flag to array.
                var allDead = new bool[Passengers.Count];
                for (var i = 0; i < Passengers.Count; i++)
                {
                    var passenger = Passengers[i];
                    allDead[i] = passenger.HealthStatus == HealthStatus.Dead;
                }

                // Determine if everybody is dead by checking if truths are greater than passenger count.
                return TrueCount(allDead) >= Passengers.Count;
            }
        }

       
        private static int TrueCount(IEnumerable<bool> booleans)
        {
            return booleans.Count(b => b);
        }

       
        public IDictionary<Entities, SimItem> Inventory => _inventory;

       
        public ReadOnlyCollection<Person.Person> Passengers => _passengers.AsReadOnly();

       
        public RationLevel Ration { get; private set; }

        
        public TravelPace Pace { get; private set; }

       
        public int Odometer { get; private set; }

     
        public int Mileage { get; private set; }

      
        public VehicleStatus Status { get; set; }

        public float Balance
        {
            get => _inventory[Entities.Cash].TotalValue;
            private set
            {
                // Skip if the quantity already matches the value we are going to set it to.
                if (value.Equals(_inventory[Entities.Cash].Quantity))
                    return;

                // Check if the value being set is zero, if so just reset it.
                if (value <= 0)
                    _inventory[Entities.Cash].Reset();
                else
                    _inventory[Entities.Cash] = new SimItem(_inventory[Entities.Cash],
                        (int) value);
            }
        }

        
        private static IEnumerable<SimItem> DefaultParts
        {
            get
            {
                // Create inventory of parts for the vehicle.
                var defaultParts = new List<SimItem>
                {
                    Parts.Wheel,
                    Parts.Axle,
                    Parts.Tongue
                };

                // Set proper quantities for each entity item type.
                foreach (var part in defaultParts)
                    switch (part.Category)
                    {
                        case Entities.Wheel:
                            // You need four (4) wheels.
                            part.ReduceQuantity(part.MaxQuantity);
                            part.AddQuantity(4);
                            break;
                        case Entities.Axle:
                            // You need one (1) axle.
                            part.ReduceQuantity(part.MaxQuantity);
                            part.AddQuantity(1);
                            break;
                        case Entities.Tongue:
                            // You need one (1) tongue.
                            part.ReduceQuantity(part.MaxQuantity);
                            part.AddQuantity(1);
                            break;
                        case Entities.Animal:
                        case Entities.Food:
                        case Entities.Clothes:
                        case Entities.Ammo:
                        case Entities.Vehicle:
                        case Entities.Person:
                        case Entities.Cash:
                        case Entities.Location:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                // Now we have default inventory of a store with all quantities zeroed out.
                return defaultParts;
            }
        }

       
        internal static IDictionary<Entities, SimItem> DefaultInventory
        {
            get
            {
                // Create inventory of items with default starting amounts.
                var defaultInventory = new Dictionary<Entities, SimItem>
                {
                    {Entities.Animal, Parts.Oxen},
                    {Entities.Clothes, Resources.Clothing},
                    {Entities.Ammo, Resources.Bullets},
                    {Entities.Wheel, Parts.Wheel},
                    {Entities.Axle, Parts.Axle},
                    {Entities.Tongue, Parts.Tongue},
                    {Entities.Food, Resources.Food},
                    {Entities.Cash, Resources.Cash}
                };

                // Zero out all of the quantities by removing their max quantity.
                foreach (var simItem in defaultInventory)
                    simItem.Value.ReduceQuantity(simItem.Value.MaxQuantity);

                // Now we have default inventory of a store with all quantities zeroed out.
                return defaultInventory;
            }
        }

      
        private int RandomMileage
        {
            get
            {
                // Total amount of monies the player has spent on animals to pull their vehicle.
                var costAnimals = Inventory[Entities.Animal].TotalValue;

                // Variables that will hold the distance we should travel in the next day.
                var totalMiles = Mileage + (costAnimals - 110)/2.5 + 10*GameSimulationApp.Instance.Random.NextDouble();

                return (int) Math.Abs(totalMiles);
            }
        }

        
        public SimItem BrokenPart { get; internal set; }

        public Person.Person PassengerLeader
        {
            get
            {
                // Leaders profession, used to determine points multiplier at end.
                Person.Person leaderPerson = null;

                // Check if passenger manifest exists.
                if (Passengers == null)
                    return null;

                // Check if there are any passengers to work with.
                if (!Passengers.Any())
                    return null;

                foreach (var person in Passengers)
                    if (person.Leader)
                        leaderPerson = person;

                return leaderPerson;
            }
        }

    
        public HealthStatus PassengerHealthStatus
        {
            get
            {
                // Check if passenger manifest exists.
                if (Passengers == null)
                    return HealthStatus.Dead;

                // Check if there are any passengers to work with, return good health if none.
                if (!Passengers.Any())
                    return HealthStatus.Dead;

                // Builds up a list of enumeration health values for living passengers.
                var livingPassengersHealth = new List<HealthStatus>();
                foreach (var person in Passengers)
                    if (person.HealthStatus != HealthStatus.Dead)
                        livingPassengersHealth.Add(person.HealthStatus);

                // Casts all the enumeration health values to integers and averages them.
                var averageHealthValue = 0;
                if (livingPassengersHealth.Count > 0)
                    averageHealthValue = (int) livingPassengersHealth.Cast<int>().Average();

                // Look for the closest health level to the average health level from all living passengers.
                var closest = ClosestTo(Enum.GetValues(typeof(HealthStatus)).Cast<int>(), averageHealthValue);
                return (HealthStatus) closest;
            }
        }

      
        public static int ClosestTo(IEnumerable<int> collection, int target)
        {
            var closest = int.MaxValue;
            var minDifference = int.MaxValue;
            foreach (var element in collection)
            {
                var difference = Math.Abs((long)element - target);
                if (minDifference <= difference)
                    continue;

                minDifference = (int)difference;
                closest = element;
            }

            return closest;
        }

        public int PassengerLivingCount
        {
            get
            {
                // Check if passenger manifest exists.
                if (Passengers == null)
                    return 0;

                // Check if there are any passengers to work with.
                if (!Passengers.Any())
                    return 0;

                // Builds up a list of enumeration health values for living passengers.
                var alivePersonsHealth = new List<HealthStatus>();
                foreach (var person in Passengers)
                    if (person.HealthStatus != HealthStatus.Dead)
                        alivePersonsHealth.Add(person.HealthStatus);

                return alivePersonsHealth.Count;
            }
        }

        
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
            if (result != 0) return result;

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
            // Only can tick vehicle on interval.
            if (systemTick)
                return;

            // Loop through all the people in the vehicle and tick them moving or ticking time or not.
            foreach (var person in _passengers)
                person.OnTick(false, skipDay);

            // Only advance the vehicle if we are actually traveling and not skipping a day of simulation.
            if ((Status != VehicleStatus.Moving) || skipDay)
                return;

            // Figure out how far we need to go to reach the next point.
            Mileage = RandomMileage;

            // Sometimes things just go slow on the trail, cut mileage in half if above zero randomly.
            if (GameSimulationApp.Instance.Random.NextBool() && (Mileage > 0))
                Mileage = Mileage/2;

            // Check for random events that might trigger regardless of calculations made.
            GameSimulationApp.Instance.EventDirector.TriggerEventByType(this, EventCategory.Vehicle);

            // Check to make sure mileage is never below or at zero.
            if (Mileage <= 0)
                Mileage = 10;

            // Use our altered mileage to affect how far the vehicle has traveled in todays tick..
            Odometer += Mileage;
        }

       
        public bool TryUseSparePart()
        {
            // Skip if the vehicle does not contain part to fix vehicle.
            if (!ContainsItem(BrokenPart))
                return false;

            // Remove one (1) of the spare parts of the broken part category.
            Inventory[BrokenPart.Category].ReduceQuantity(1);

            // Repair the vehicle.
            RepairAllParts();

            // Returns true so the calling method knows the vehicle is now repaired.
            return true;
        }

        public void BreakRandomPart()
        {
            // Skip if there is already a broken part.
            if (BrokenPart != null)
                return;

            // Randomly select one of the parts to break in the vehicle.
            var randomPartIndex = GameSimulationApp.Instance.Random.Next(_parts.Count);

            // Sets the broken part for other processes to deal with.
            BrokenPart = _parts[randomPartIndex];
        }

      
        internal void ReduceMileage(int amount)
        {
            // Mileage cannot be reduced when parked.
            if (Status != VehicleStatus.Moving)
                return;

            // Check if current mileage is below zero.
            if (Mileage <= 0)
                return;

            // Calculate new mileage.
            var updatedMileage = Mileage - amount;

            // Check if updated mileage is below zero.
            if (updatedMileage <= 0)
                updatedMileage = 0;

            // Check that mileage doesn't already exist as this value somehow.
            if (!updatedMileage.Equals(Mileage))
                Mileage = updatedMileage;
        }

      
        public void ChangePace(TravelPace castedSpeed)
        {
            // Change game simulation speed.
            Pace = castedSpeed;
        }

       
        public void AddPerson(Person.Person person)
        {
            _passengers.Add(person);
        }

        public void Purchase(SimItem transaction)
        {
            // Check of the player can afford this item.
            if (Balance < transaction.TotalValue)
                return;

            // Create new item based on old one, with new quantity value from store, trader, random event, etc.
            Balance -= transaction.TotalValue;

            // Make sure we add the quantity and not just replace it.
            _inventory[transaction.Category].AddQuantity(transaction.Quantity);
        }

       
        public void ResetVehicle(int startingMonies = 0)
        {
            // Parts for the vehicle to keep it in working order and moving.
            _parts = new List<SimItem>(DefaultParts);

            // Inventory items for the passengers to use like food, clothes, spare parts.
            _inventory = new Dictionary<Entities, SimItem>(DefaultInventory);

            // Passengers the vehicle will be moving along the trail.
            _passengers = new List<Person.Person>();

            // Money the passengers use collectively to purchase items.
            Balance = startingMonies;

            // Determines amount of food consumed per day.
            Ration = RationLevel.Filling;

            // Number of miles the vehicle has traveled.
            Odometer = 0;

            // Vehicle is not moving and currently stopped.
            Status = VehicleStatus.Stopped;
        }

      
        public void ChangeRations(RationLevel ration)
        {
            Ration = ration;
        }

     
        public static SimItem CreateRandomItem()
        {
            // Loop through the inventory and decide which items to give free copies of.
            foreach (var itemPair in DefaultInventory)
            {
                // Determine if we will be making more of this item, if it's the last one then we have to.
                if (GameSimulationApp.Instance.Random.NextBool())
                    continue;

                // Skip certain items that cannot be traded.
                switch (itemPair.Value.Category)
                {
                    case Entities.Food:
                    case Entities.Clothes:
                    case Entities.Ammo:
                    case Entities.Wheel:
                    case Entities.Axle:
                    case Entities.Tongue:
                    case Entities.Vehicle:
                    case Entities.Animal:
                    case Entities.Person:
                    {
                        // Create a random number within the range we need to create an item.
                        var amountToMake = itemPair.Value.MaxQuantity/4;

                        // Check if created amount goes above ceiling.
                        if (amountToMake > itemPair.Value.MaxQuantity)
                            amountToMake = itemPair.Value.MaxQuantity;

                        // Check if created amount goes below floor.
                        if (amountToMake <= 0)
                            amountToMake = 1;

                        // Add some random amount of the item from one to total amount.
                        var createdAmount = GameSimulationApp.Instance.Random.Next(1, amountToMake);

                        // Create a new item with generated quantity.
                        var createdItem = new SimItem(itemPair.Value, createdAmount);
                        return createdItem;
                    }
                    case Entities.Cash:
                    case Entities.Location:
                        continue;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // Default response is to return a NULL item if something terrible happens.
            return null;
        }

   
        public IDictionary<Entities, int> CreateRandomItems()
        {
            // Items that will be created by this method.
            IDictionary<Entities, int> createdItems = new Dictionary<Entities, int>();

            // Make a copy of the inventory to iterate over.
            var copiedInventory = new Dictionary<Entities, SimItem>(Inventory);

            // Loop through the inventory and decide which items to give free copies of.
            foreach (var itemPair in copiedInventory)
            {
                // Skip item if quantity is at maximum.
                if (itemPair.Value.Quantity >= itemPair.Value.MaxQuantity)
                    continue;

                // Determine if we will be making more of this item.
                if (GameSimulationApp.Instance.Random.NextBool())
                    continue;

                // Add some random amount of the item from one to total amount.
                var createdAmount = GameSimulationApp.Instance.Random.Next(1, itemPair.Value.MaxQuantity/4);

                // Add the amount ahead of time so we can figure out of it is above maximum.
                var simulatedAmountAdd = itemPair.Value.Quantity + createdAmount;

                // Adjust the simulated added amount to item max quantity if it ended up being above it.
                if (simulatedAmountAdd >= itemPair.Value.MaxQuantity)
                    simulatedAmountAdd = itemPair.Value.MaxQuantity;

                // Add the amount we created to total of actual item in inventory.
                Inventory[itemPair.Key] = new SimItem(itemPair.Value, simulatedAmountAdd);

                // Tabulate the amount we created in dictionary to be returned to caller.
                createdItems.Add(itemPair.Key, createdAmount);
            }

            // Clear out the copied list we made for iterating.
            copiedInventory.Clear();

            // Return the created item summary.
            return createdItems;
        }

      
        public IDictionary<Entities, int> DestroyRandomItems()
        {
            // Dictionary that will keep track of enumeration item type and destroyed amount for record keeping purposes.
            IDictionary<Entities, int> destroyedItems = new Dictionary<Entities, int>();

            // Make a copy of the inventory to iterate over.
            var copiedInventory = new Dictionary<Entities, SimItem>(Inventory);

            // Loop through the inventory and decide to randomly destroy some inventory items.
            foreach (var itemPair in copiedInventory)
            {
                // Skip item if quantity is less than one.
                if (itemPair.Value.Quantity < 1)
                    continue;

                // Determine if we will be destroying this item.
                if (GameSimulationApp.Instance.Random.NextBool())
                    continue;

                // Destroy some random amount of the item from one to total amount.
                var destroyAmount = GameSimulationApp.Instance.Random.Next(1, itemPair.Value.Quantity);

                // Remove the amount we destroyed from the actual inventory.
                Inventory[itemPair.Key].ReduceQuantity(destroyAmount);

                // Tabulate the amount we destroyed in dictionary to be returned to caller.
                destroyedItems.Add(itemPair.Key, destroyAmount);
            }

            // Clear out the copied list we made for iterating.
            copiedInventory.Clear();

            // Return the destroyed item summary.
            return destroyedItems;
        }

     
        public bool ContainsItem(SimItem wantedItem)
        {
            // Loop through vehicle inventory.
            foreach (var simItem in Inventory)
                if ((simItem.Value.Name == wantedItem.Name) && (simItem.Value.Category == wantedItem.Category) &&
                    (simItem.Value.Quantity >= wantedItem.MinQuantity))
                    return true;

            return false;
        }

        
        private void RepairAllParts()
        {
            // Loop through every part in the vehicle and repair it.
            foreach (var part in _parts)
                part.Repair();

            // Set the vehicle status to be stopped.
            Status = VehicleStatus.Stopped;
        }

      
        public void CheckStatus()
        {
            // Checks if the player has animals to pull their vehicle.
            if (Inventory[Entities.Animal].Quantity <= 0)
            {
                Status = VehicleStatus.Disabled;
                return;
            }

            // Don't change the state from disabled to moving.
            if (Status == VehicleStatus.Disabled)
                return;

            // Default response it to allow the vehicle to move.
            Status = VehicleStatus.Moving;
        }
    }
}