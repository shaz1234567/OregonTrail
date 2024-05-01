

using System.Collections.Generic;
using OregonTrailDotNet.Entity.Person;

namespace OregonTrailDotNet.Entity.Vehicle
{
    
    public static class VehicleExtensions
    {
        
        public static IEnumerable<Person.Person> TryKill(this IEnumerable<Person.Person> passengers)
        {
            // Determine if we lost any people, this is separate from items in vehicle.
            var peopleKilled = new List<Person.Person>();
            foreach (var person in passengers)
            {
                // It all comes down to a dice roll if the storm kills you.
                if (!GameSimulationApp.Instance.Random.NextBool() ||
                    (person.HealthStatus == HealthStatus.Dead))
                    continue;

                // Kills the person and adds them to list.
                person.Kill();
                peopleKilled.Add(person);
            }

            // Gives back the list of people that were killed by this extension method.
            return peopleKilled;
        }

        public static void Damage(this IList<Person.Person> passengers, int amount)
        {
            // Check if there are people to damage.
            if (passengers.Count <= 0)
                return;

            // Check if the amount is greater than zero.
            if (amount <= 0)
                return;

            // Check if the amount is greater than passenger count.
            if (amount <= passengers.Count)
                return;

            // Figure out how much damage needs to be applied to each person.
            var damagePerPerson = amount/passengers.Count;

            // Loop through all the passengers and damage them according to calculated amount.
            foreach (var person in passengers)
            {
                // Skip if the person is already dead.
                if (person.HealthStatus == HealthStatus.Dead)
                    continue;

                // Apply damage to the person we calculated above.
                person.Damage(damagePerPerson);
            }
        }
    }
}